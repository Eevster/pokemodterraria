using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Pets;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class ShadowBall : PokemonAttack
    {
        private Vector2 shotVelocity;
        private int hitStun = 0;
        private int hitCount = 0;
        private static Asset<Texture2D> auraTexture;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void Load()
        {
            auraTexture = ModContent.Request<Texture2D>("Pokemod/Content/Projectiles/PokemonAttackProjs/ShadowBallAura");
        }

        public override void Unload()
        {
            auraTexture = null;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(hitStun);
            writer.Write(hitCount);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            hitStun = reader.ReadInt32();
            hitCount = reader.ReadInt32();
            base.ReceiveExtraAI(reader);
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.timeLeft = 80;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;

            Projectile.scale = 0.7f;

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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack)
                {
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, 10f * Vector2.Normalize(targetCenter - pokemon.Center), ModContent.ProjectileType<ShadowBall>(), 0, 2f, pokemon.owner, pokemonOwner.GetPokemonAttackDamage(GetType().Name))];
                            SoundEngine.PlaySound(SoundID.Item77, pokemon.position);
                            pokemonOwner.canAttackOutTimer = false;
                            break;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Projectile.ModProjectile.Texture).Value;
            SpriteEffects mirroring = SpriteEffects.None;
            if (shotVelocity.X < 0)
            {
                mirroring = SpriteEffects.FlipHorizontally;
            }

            Main.EntitySpriteDraw(auraTexture.Value, Projectile.Center - Main.screenPosition,
                auraTexture.Frame(1, 5, 0, Projectile.frame), lightColor, Projectile.rotation,
                auraTexture.Frame(1, 5).Size() / 2f, Projectile.scale, mirroring, 0);

            int brightness = (int)(255 * (1f - 0.9f * hitStun / (Projectile.localNPCHitCooldown - 1)));

            Main.EntitySpriteDraw(texture, (Projectile.Center - Main.screenPosition),
                texture.Bounds, new Color(brightness, brightness, brightness),
                0f, texture.Frame(1, 1).Size() / 2f, 1f, mirroring, 0);

            return false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            shotVelocity = Projectile.velocity;

            base.OnSpawn(source);
        }

        public override void AI()
        {
            if (hitCount >= 3)
            {
                Projectile.Kill();
            }
            if (hitStun == Projectile.localNPCHitCooldown - 1)
            {
                hitCount++;
            }
            if (hitStun > 0)
            {
                Projectile.velocity = Vector2.Zero;
                Projectile.damage = 0;
                Projectile.timeLeft++;
                hitStun--;
                if (hitStun > Projectile.localNPCHitCooldown * 0.7f)
                {
                    Projectile.scale += 0.2f / (Projectile.localNPCHitCooldown * 0.3f);
                }
            }
            else
            {
                Projectile.velocity = shotVelocity;
                Projectile.damage = (int)Projectile.ai[0];
                Projectile.scale = 0.6f + 0.2f * (hitCount);
            }

            Projectile.rotation -= (shotVelocity.X >= 0 ? 1 : -1) * MathHelper.ToRadians(10);

            int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6));
            Main.dust[dustIndex].noGravity = true;

            UpdateAnimation();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        private void UpdateAnimation()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= (Main.projFrames[Projectile.type]))
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            HitStun();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            base.OnHitPlayer(target, info);
            HitStun();
        }

        public void HitStun()
        {
            PokemonPetProjectile pokemonOwner = (PokemonPetProjectile)pokemonProj.ModProjectile;
            int attackDamage = pokemonOwner.GetPokemonAttackDamage(GetType().Name);

            hitStun = Projectile.localNPCHitCooldown - 1;
            Projectile.scale = 0.6f + 0.35f * (hitCount);

            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item72, Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, Main.rand.Next(-5, 6), Main.rand.Next(-5, 6), 0, new Color(255, 255, 255), 1.5f);
                Main.dust[dustIndex].noGravity = true;
            }

            base.OnKill(timeLeft);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float radius = 22.5f * Projectile.scale;
            Vector2 start = Projectile.Center - radius * new Vector2(0, -1).RotatedBy(Projectile.rotation);
            Vector2 end = Projectile.Center + radius * new Vector2(0, -1).RotatedBy(Projectile.rotation);
            float collisionPoint = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 32f, ref collisionPoint);
        }


    }
}
