using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class SludgeBomb : PokemonAttack
    {
        private int bounceCounter = 0;

        public override void SetDefaults()
        {

            Projectile.timeLeft = 150;

            Projectile.width = 36;
            Projectile.height = 36;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
            Projectile.penetrate = -1;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
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
                            float shootSpeed = 12f;
                            float timeToTarget = 0;
                            Vector2 shootVelocity = shootSpeed * Vector2.Normalize(targetCenter - pokemon.Center);
                            if (shootVelocity.X != 0f)
                            {
                                timeToTarget = (targetCenter - pokemon.Center).X / shootVelocity.X;
                            }
                            if (timeToTarget > pokemonOwner.distanceToAttack / shootSpeed || timeToTarget == 0)
                            {
                                timeToTarget = pokemonOwner.distanceToAttack / shootSpeed;
                            }
                            shootVelocity.Y -= timeToTarget * 0.35f;

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<SludgeBomb>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 5f, pokemon.owner, 0)];
                            SoundEngine.PlaySound(SoundID.Item21 with { Pitch = -0.3f }, pokemon.position);
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

        public override void AI()
        {
            if (++Projectile.frameCounter == 8)
            {
                var pokemonOwner = (PokemonPetProjectile)pokemonProj.ModProjectile;

                Projectile.damage = pokemonOwner.GetPokemonAttackDamage(GetType().Name);
            }

            //Gravity
            Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 20f)
            {
                Projectile.velocity.Y = 20f;
            }

            //Rolling
            Projectile.rotation += MathHelper.ToRadians(Projectile.velocity.X);

            DustTrail(Projectile.velocity);

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
        }

        private void DustTrail(Vector2 velocity)
        {
            switch (Main.rand.Next(0, 7))
            {
                case 0:
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice_Purple, 0, 0, default, default, 1.5f); break;
                case 1:
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Venom, velocity.X * 0.5f, velocity.Y * 0.5f); break;
                case 2:
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Venom, velocity.X * 0.4f, velocity.Y * 0.4f);
                    SoundEngine.PlaySound(SoundID.SplashWeak with { Pitch = -0.7f }, Projectile.position); break;
                case 3:
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice_Purple, velocity.X * 0.2f, velocity.Y * 0.2f, default, default, 2f);
                    Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Venom, velocity.X * 0.2f, velocity.Y * 0.2f, default, default, 2f); break;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 6;
            height = 6;
            fallThrough = true;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            DustBomb(oldVelocity, Projectile.Center);
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            DustBomb(Projectile.velocity, target.Center);
            base.OnHitPlayer(target, info);
        }

        private void DustBomb(Vector2 velocity, Vector2 targetPosition)
        {
            if(Projectile.timeLeft <= 5) //Does repeat if already exploded.
            {
                return;
            }
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Ice_Purple, Main.rand.Next(-2, 3), -3, default, default, 2f);
            Dust.NewDust(Projectile.Center, Projectile.width, Projectile.height, DustID.Venom, Main.rand.Next(-2, 3), -3, default, default, 2f);

            Dust.NewDust(targetPosition, Projectile.width, Projectile.height, DustID.Ice_Purple, Main.rand.Next(-2, 3) + velocity.X * 0.1f, -3 + velocity.Y * 0.1f, default, default, 2f);
            Dust.NewDust(targetPosition, Projectile.width, Projectile.height, DustID.Water_Corruption, Main.rand.Next(-2, 3) + velocity.X * 0.1f, -3 + velocity.Y * 0.1f, default, default, 2f);

            if (Projectile.ai[0] == 0) //Primary Projectile
            {
                //AOE damage for initial hit
                Vector2 oldCenter = Projectile.Center;
                Projectile.width *= 4; Projectile.height *= 4;
                Projectile.position = oldCenter - new Vector2(Projectile.width / 2, Projectile.height / 2);
                
                SoundEngine.PlaySound(SoundID.Item81, targetPosition);

                //Fires child projectiles, correctly assigned to the parent pokemon.
                var pokemon = pokemonProj;
                var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
                if (pokemon.owner == Main.myPlayer)
                {
                    int remainProjs = 4;
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            float shootSpeed = 12f;
                            float xVelocityMod = (Math.Abs(remainProjs - 2.5f) > 0.5 ? 0.575f : 0.18f) * Math.Sign(remainProjs - 2.5f);
                            Vector2 shootVelocity = shootSpeed * Vector2.Normalize(new Vector2(xVelocityMod, -1f));

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), Projectile.Center, shootVelocity, ModContent.ProjectileType<SludgeBomb>(), 0, 5f, pokemon.owner, 1)];
                            remainProjs--;
                            if (remainProjs <= 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else //Child Projectiles
            {
                SoundEngine.PlaySound(SoundID.Shimmer1, targetPosition);
            }
            Projectile.timeLeft = 5; //Allows some time before projectile dies so damage can be done with the new AOE hitbox.
        }
    }
}
