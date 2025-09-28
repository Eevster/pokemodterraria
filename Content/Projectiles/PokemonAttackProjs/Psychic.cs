using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using ReLogic.Content;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class Psychic : PokemonAttack
    {
        bool exploded = false;
        private int idleTimer = 0;
        private static Asset<Texture2D> explosionTexture;
        public int pokemonDamage;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void Load()
        {
            explosionTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/PsychicBlast");
        }

        public override void Unload()
        {
            explosionTexture = null;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(exploded);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            exploded = reader.ReadBoolean();
            base.ReceiveExtraAI(reader);
        }
        public override void SetDefaults()
        {
            Projectile.timeLeft = 120;

            Projectile.width = 26;
            Projectile.height = 26;

            Projectile.light = 0.2f;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = false;
            Projectile.penetrate = 6;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 17;
            Projectile.stopsDealingDamageAfterPenetrateHits = true;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if (pokemon.owner == Main.myPlayer)
            {
                for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                {
                    if (pokemonOwner.attackProjs[i] == null)
                    {
                        pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
                        pokemonOwner.timer = pokemonOwner.attackDuration;
                        pokemonOwner.canAttack = false;
                        pokemonOwner.canAttackOutTimer = true;
                        break;
                    }
                }
            }
        }

        public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter)
        {
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

            if (pokemon.owner == Main.myPlayer)
            {
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20)
                {
                    int remainProjs = 1;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, Vector2.Zero, ModContent.ProjectileType<Psychic>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner, pokemonOwner.GetPokemonAttackDamage(GetType().Name))];
                            SoundEngine.PlaySound(SoundID.Item42, pokemon.position);
                            remainProjs--;
                            pokemonOwner.canAttackOutTimer = false;
                            if (remainProjs <= 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture).Value;
            if (!exploded)
            {
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                texture.Frame(1, 5, 0, Projectile.frame), Color.White, 0,
                texture.Frame(1, 5).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

                return false;
            }
            else
            {
                Main.EntitySpriteDraw(explosionTexture.Value, Projectile.Center - Main.screenPosition,
                    explosionTexture.Frame(1, 8, 0, Projectile.frame), Color.White, 0,
                    explosionTexture.Frame(1, 8).Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

                return false;
            }

        }

        public override void AI()
        {
            if (Projectile.timeLeft == 1 && !exploded) //transition to explosion
            {
                Explode();
            }

            if (!exploded)
            {
                HomingMovement();
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }

            if (Projectile.velocity.Length() < 0.01f && !exploded)
            {
                if (idleTimer++ > 15)
                {
                    Explode();
                }
            }
            else
            {
                idleTimer = 0;
            }

            UpdateAnimation();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.life > damageDone) 
            {
                Explode();
                Projectile.Center = target.Center;
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (target.statLife > info.Damage)
            {
                Explode();
                Projectile.Center = target.Center;
            }
            base.OnHitPlayer(target, info);
        }

        private void Explode()
        {
            exploded = true;
            Projectile.frame = 0;
            Projectile.frameCounter = 0;
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1;
            Projectile.damage = (int)Projectile.ai[0]; //need to re-assign damage after "Projectile.stopsDealingDamageAfterPenetrateHits" triggers and sets damage to 0.
            SoundEngine.PlaySound(SoundID.Item24, Projectile.position);
        }

        public void HomingMovement()
        {
            Vector2 homingTarget = Projectile.Center;

            foundTarget = false;
            if (attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack)
            {
                SearchTarget((float)PokemonData.pokemonAttacks["Psychic"].distanceToAttack);
                if (foundTarget)
                {
                    if (targetEnemy != null)
                    {
                        if (targetEnemy.active)
                        {
                            homingTarget = targetEnemy.Center;
                        }
                    }
                    else if (targetPlayer != null)
                    {
                        if (targetPlayer.active)
                        {
                            homingTarget = targetPlayer.Center;
                        }
                    }
                }
            }
            else if (attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack)
            {
                if (Trainer.targetPlayer != null)
                {
                    homingTarget = Trainer.targetPlayer.Center;
                }
                else if (Trainer.targetNPC != null)
                {
                    homingTarget = Trainer.targetNPC.Center;
                }
                else
                {
                    homingTarget = Trainer.attackPosition;
                }
            }

            float currentSpeed = 20f * (float)Math.Pow(1.0f - (float)Projectile.timeLeft / 120f, 1f);
            float homingStrength = 0.03f;
            if(Projectile.timeLeft < 70f)
            {
                homingStrength = 0.1f;
            }
            if (Projectile.timeLeft < 40f)
            {
                homingStrength = 0.5f;
            }
            if (Projectile.velocity.Length() > 1f || (homingTarget - Projectile.Center).Length() > float.Epsilon)
            {
                Projectile.velocity *= 1 - homingStrength;
                Projectile.velocity += homingStrength * currentSpeed * (homingTarget - Projectile.Center).SafeNormalize(Vector2.Zero);
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= (exploded ? 8 : Main.projFrames[Projectile.type]))
                {
                    if (exploded)
                    {
                        Projectile.Kill();
                    }
                    Projectile.frame = 0;
                    int dustIndex = Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Gastropod);
                    Main.dust[dustIndex].noGravity = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item92, Projectile.position);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 start = Projectile.Center + new Vector2(exploded ? 62 : 26, 0);
            Vector2 end = Projectile.Center - new Vector2(exploded ? 62 : 26, 0);
            float collisionPoint = 0f; // Don't need that variable, but required as parameter

            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, Projectile.scale * (exploded ? 124f : 52f), ref collisionPoint);
        }
    }
}

