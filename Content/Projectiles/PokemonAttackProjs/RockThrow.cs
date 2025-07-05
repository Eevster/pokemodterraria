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
	public class RockThrow : PokemonAttack
	{
		private int bounceCounter = 0;

        public override void SetDefaults()
        {

            Projectile.timeLeft = 150;
			
			Projectile.width = 26;
            Projectile.height = 26;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.tileCollide = true;  
            Projectile.penetrate = 5;

            Projectile.aiStyle = -1; 
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            base.SetDefaults();
        }

        public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;

			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						pokemonOwner.canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
            var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
            
			if(pokemon.owner == Main.myPlayer){
				if(pokemonOwner.currentStatus == (int)PokemonPetProjectile.ProjStatus.Attack && pokemonOwner.timer <= 20){
					int remainProjs = 1;
					for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
						if(pokemonOwner.attackProjs[i] == null){
							float shootSpeed = 15f;
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

                            pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, shootVelocity, ModContent.ProjectileType<RockThrow>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 5f, pokemon.owner)];
							SoundEngine.PlaySound(SoundID.Item1, pokemon.position);
							remainProjs--;
							pokemonOwner.canAttackOutTimer = false;
							if(remainProjs <= 0){
								break;
							}
						}
					} 
				}
			}
		}

        public override void AI()
        {
			//Gravity
			Projectile.velocity.Y += 0.7f;
            if (Projectile.velocity.Y > 10f)
            {
                Projectile.velocity.Y = 10f;
            }
			
			//Fade out
            if (Projectile.timeLeft < 20){
                Projectile.Opacity = 1f * Projectile.timeLeft * 0.05f;
            }

			//Despawn when not moving
			if(Projectile.velocity.Length() < 1f)
			{
				Projectile.timeLeft = 20;
			}

			if(Projectile.timeLeft <= 0 || bounceCounter >= 3)
			{
				Projectile.Kill();
            }

            if (Projectile.owner == Main.myPlayer)
            {
                Projectile.netUpdate = true;
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
			float speedLoss = 0.85f;
			int dustAmount = 5;
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for (int i = 0; i < dustAmount; i++)
			{
				Dust.NewDust(Projectile.Center, 6, 6, DustID.Stone, -0.2f * Projectile.velocity.X, -0.2f * Projectile.velocity.Y);
			}
			
            Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);

            // If the projectile hits the left or right side of the tile, reverse the X velocity
            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
            {
                Projectile.velocity.X = -oldVelocity.X * speedLoss;
				bounceCounter++;
            }

            // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y * speedLoss;
                bounceCounter++;
            }
			
            return false;
		}
    }
}
