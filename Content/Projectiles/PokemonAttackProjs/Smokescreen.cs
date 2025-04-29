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
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Projectiles.PokemonAttackProjs
{
	public class Smokescreen : PokemonAttack
	{
        const int explosionSize = 80;
		float explosionScale = 1f;
		bool exploded = false;
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.hostile = false; 
			Projectile.DamageType = DamageClass.Ranged; 
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true; 
			Projectile.light = 0.5f;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;

			Projectile.penetrate = -1;
			base.SetDefaults();
		}

		public override void Attack(Projectile pokemon, float distanceFromTarget, Vector2 targetCenter){
			var pokemonOwner = (PokemonPetProjectile)pokemon.ModProjectile;
			
			if(pokemon.owner == Main.myPlayer){
				for(int i = 0; i < pokemonOwner.nAttackProjs; i++){
					if(pokemonOwner.attackProjs[i] == null){
						float gravity = 0.25f;
						float targetDistanceY = -targetCenter.Y+pokemon.Center.Y;
						float throwSpeedY = System.Math.Clamp(targetDistanceY/20f,3f,30f);

						double delta = throwSpeedY*throwSpeedY-2f*gravity*targetDistanceY;

						if(delta<0){
							break;
						}

						double timeToReach = (float)(2f*throwSpeedY+(-throwSpeedY+System.Math.Sqrt(delta)))/gravity;
						float targetDistanceX = (float)(targetCenter.X-pokemon.Center.X);
						float throwSpeedX = (float)(targetDistanceX/timeToReach);

						Vector2 projSpeed = new Vector2(throwSpeedX,-throwSpeedY);

						pokemonOwner.attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(pokemon), pokemon.Center, projSpeed, ModContent.ProjectileType<Smokescreen>(), pokemonOwner.GetPokemonAttackDamage(GetType().Name), 0f, pokemon.owner)];
						pokemonOwner.currentStatus = (int)PokemonPetProjectile.ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item20, pokemon.position);
						pokemonOwner.timer = pokemonOwner.attackDuration;
						pokemonOwner.canAttack = false;
						break;
					}
				} 
			}
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, Projectile.velocity.X / 2, Projectile.velocity.Y / 2, 0, Color.Black, 0f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].scale = (float)Main.rand.Next(100, 125) * 0.008f;

			if(!exploded){
				Projectile.velocity.Y += 0.25f;
			}else{
                float explosionFinalSize = explosionScale*explosionSize;
                for(int g = 0; g < 2; g++)
                {
                    int goreIndex = Gore.NewGore(Projectile.InheritSource(Projectile), Projectile.Center + new Vector2(Main.rand.NextFloat(-explosionFinalSize*0.5f, explosionFinalSize*0.5f), Main.rand.NextFloat(-explosionFinalSize*0.5f, explosionFinalSize*0.5f)), default(Vector2), 99, 1f);
                    Main.gore[goreIndex].scale = Main.rand.NextFloat(0.5f, 1f);
                    Main.gore[goreIndex].velocity *= 0.5f;
                }
            }
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			if(!exploded){
				Explode();
			}
            base.OnHitNPC(target, hit, damageDone);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(!exploded){
				Explode();
			}else{
                target.AddBuff(BuffID.Darkness, 3*60);
            }
            base.OnHitPlayer(target, info);
        }

		public override bool OnTileCollide (Vector2 oldVelocity){
			if(!exploded){
				Explode();
			}

			return false;
		}

        private void Explode(){
			Projectile.timeLeft = 90;
            Projectile.velocity = Vector2.Zero;
            exploded = true;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			Vector2 start = Projectile.oldPosition + 0.5f*new Vector2(Projectile.width, Projectile.height) - 0.5f*Projectile.width*Projectile.velocity.SafeNormalize(Vector2.UnitX);
			Vector2 end = Projectile.Center + 0.5f*Projectile.width*Projectile.velocity.SafeNormalize(Vector2.UnitX);
			float collisionPoint = 0f; // Don't need that variable, but required as parameter
			if(exploded){
				start = Projectile.Center-new Vector2(explosionScale*explosionSize*0.5f, 0);
				end = Projectile.Center+new Vector2(explosionScale*explosionSize*0.5f, 0);
			}
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, exploded?explosionScale*explosionSize:Projectile.height, ref collisionPoint);
		}
    }
}
