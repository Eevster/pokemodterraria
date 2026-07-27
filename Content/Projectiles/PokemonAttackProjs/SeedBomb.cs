using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using Pokemod.Content.Buffs;
using Pokemod.Content.NPCs.PokemonNPCs;
using Pokemod.Content.Pets;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
    public class SeedBomb : PokemonAttack
    {
        public override void SetDefaults()
        {
            Projectile.timeLeft = 150;

            Projectile.width = 24;
            Projectile.height = 24;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;
            Projectile.penetrate = 1;

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
                        pokemonOwner.remainAttacks = 3;
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
                if (pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer%12==0)
                {
                    for (int i = 0; i < pokemonOwner.nAttackProjs; i++)
                    {
                        if(pokemonOwner.remainAttacks <= 0){
							pokemonOwner.canAttackOutTimer = false;
							break;
						}
                        if (pokemonOwner.attackProjs[i] == null)
                        {
                            float shootSpeed = 12f;
                            float timeToTarget = 0;
                            targetCenter += GetAuxPositionForMovingTarget(targetCenter, Main.rand.Next(0,60));
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

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<SeedBomb>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 2f, pokemon.owner)];
                            SoundEngine.PlaySound(SoundID.Item5, pokemon.position);
                            pokemonOwner.remainAttacks--;
							break;
                        }
                    }
                }
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = (int)(Projectile.damage*0.33f);
            base.OnSpawn(source);
        }

        public override void AI()
        {
            //Gravity
            Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 20f)
            {
                Projectile.velocity.Y = 20f;
            }

            if(Projectile.velocity.Length() > 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver4;
            }

            DustTrail();

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
            }
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

        public override void OnHitPokemonPet(PokemonPetProjectile target, int damageDone)
        {
            DustBomb(Projectile.velocity, target.Projectile.Center);
            base.OnHitPokemonPet(target, damageDone);
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
			base.OnTileCollide(oldVelocity);
			return true;
		}

        private void DustTrail()
        {
            Dust.NewDust(Projectile.Center, 0, 0, DustID.Sluggy, Scale: 1f);
            if (Main.rand.NextBool(10))
            {
                SoundEngine.PlaySound(SoundID.SplashWeak with { Volume = 0.3f}, Projectile.position);
            }
        }

        private void DustBomb(Vector2 velocity, Vector2 targetPosition)
        {   
            for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(Projectile.Center - 0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.Sluggy, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), default, default, 1.5f);
                Dust.NewDust(targetPosition - 0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.Sluggy, Main.rand.Next(-2, 3) + velocity.X * 0.1f, Main.rand.Next(-2, 3) + velocity.Y * 0.1f, default, default, 1f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            base.OnKill(timeLeft);
        }
    }
}
