using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets.EeveePet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.EeveePetShiny
{
	public class EeveePetProjectileShiny : PokemonPetProjectile
	{
		public override int nAttackProjs => 4;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<EeveePetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;

		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 50;
			Projectile.height = 40;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
			// Default movement parameters (here for attacking)
			float speed = 5f;
			float inertia = 20f;

			float maxFallSpeed = 10f;

			if (foundTarget) {
				if(distanceFromTarget > 400f){
					Vector2 direction = targetCenter - Projectile.Center;
					direction.Normalize();
					direction *= speed;

					Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + direction) / inertia).X;

					if((targetCenter - Projectile.Center).Y < 0 && -(targetCenter - Projectile.Center).Y > Math.Abs((targetCenter - Projectile.Center).X)){
						if(Math.Abs(Projectile.velocity.Y) < float.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
							currentStatus = (int)ProjStatus.Jump;
							Projectile.velocity.Y -= (int)Math.Sqrt(2*0.3f*Math.Clamp(Math.Abs((targetCenter - Projectile.Center).Y),0,96));
						}
					}
				}else{
					if(distanceFromTarget > 200f){
						Vector2 direction = targetCenter - Projectile.Center;
						direction.Normalize();
						direction *= speed/2;

						Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + direction) / inertia).X;
					}else{
						Projectile.velocity.X *= 0.95f;
					}
				}
				if(distanceFromTarget < 800f){
					if(timer <= 0){
						if(canAttack){
							SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
							if(Projectile.owner == Main.myPlayer){
								for(int i = 0; i < nAttackProjs; i++){
									if(attackProjs[i] == null){
										attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Swift>(), GetPokemonDamage(), 2f, Projectile.owner, i*MathHelper.PiOver2)];
									}
								} 
							}
							timer = attackDuration;
							canAttack = false;
						}
					}
				}

				if(Projectile.owner == Main.myPlayer){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] != null){
							if(attackProjs[i].active){
								if(attackProjs[i].ai[1] == 0){
									attackProjs[i].Center = Projectile.position + new Vector2(25,23) + 50*new Vector2(1,0).RotatedBy(attackProjs[i].ai[0]);
								}
							}else{
								attackProjs[i] = null;
							}
						}
					}
				}

				if(timer <= 0){
					if(!canAttack){
						canAttack = true;
						timer = attackCooldown;
					}
				}
			}
			else {
				if(!canAttack){
					canAttack = true;
					timer = attackCooldown;
				}
				if(Projectile.owner == Main.myPlayer){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] != null){
							if(attackProjs[i].active){
								if(attackProjs[i].ai[1] != 0){
									attackProjs[i] = null;
								}
							}else{
								attackProjs[i] = null;
							}
						}
					}
				}
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 600f) {
					// Speed up the minion if it's away from the player
					speed = 8f;
				}
				else {
					// Slow down the minion if closer to the player
					speed = 5f;
				}

				if (distanceToIdlePosition > 80f) {
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speed;
					Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia).X;
				}else{
					if(Math.Abs(Projectile.velocity.X)>0.2f){
						Projectile.velocity.X *= 0.9f;
					}else{
						Projectile.velocity.X = 0;
					}
				}
			}

			if(currentStatus != (int)ProjStatus.Jump){
				if(Math.Abs(Projectile.velocity.X) < float.Epsilon){
					currentStatus = (int)ProjStatus.Idle;
				}else{
					currentStatus = (int)ProjStatus.Walk;
				}
			}

			if(Projectile.velocity.Y > 0.04f){
				currentStatus = (int)ProjStatus.Fall;
			}

			if(Math.Abs(Projectile.velocity.Y) < float.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
				Jump();
			}

			if(timer > 0){
				timer--;
			}

			Projectile.velocity.Y += 0.2f;
			if(Projectile.velocity.Y > maxFallSpeed){
				Projectile.velocity.Y = maxFallSpeed;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 22;
			height = 30;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
