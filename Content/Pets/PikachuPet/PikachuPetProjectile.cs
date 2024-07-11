using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PikachuPet
{
	public class PikachuPetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 1;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<PikachuPetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 30;
		public override int attackCooldown => 60;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 48;
			Projectile.height = 36;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0.3f;
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
						if(Math.Abs(Projectile.velocity.Y) < Double.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
							currentStatus = (int)ProjStatus.Jump;
							Projectile.velocity.Y -= (int)Math.Sqrt(2*0.3f*Math.Clamp(Math.Abs((targetCenter - Projectile.Center).Y),0,160));
						}
					}
				}
				if(distanceFromTarget < 600f){
					if(timer <= 0){
						if(canAttack){
							currentStatus = (int)ProjStatus.Attack;
							SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
							if(Projectile.owner == Main.myPlayer){
								attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ThunderboltHold>(), GetPokemonDamage(), 2f, Projectile.owner)];
							}
							timer = attackDuration;
							canAttack = false;
						}
					}
				}
				if(timer <= 0){
					if(!canAttack){
						if(currentStatus == (int)ProjStatus.Attack){
							currentStatus = (int)ProjStatus.Idle;
						}
						canAttack = true;
						timer = attackCooldown;
					}
				}
				if(attackProjs[0] != null){
					if(attackProjs[0].active){
						Projectile.velocity.X *= 0.9f;
						maxFallSpeed = 2f;
						attackProjs[0].Center = Projectile.Center;
					}else{
						attackProjs[0] = null;
					}
				}
			}
			else {
				if(currentStatus == (int)ProjStatus.Attack){
					currentStatus = (int)ProjStatus.Idle;
					canAttack = true;
					timer = attackCooldown;
				}
				if(attackProjs[0] != null){
					if(attackProjs[0].active){
						attackProjs[0].Kill();
					}
					attackProjs[0] = null;
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

			if(currentStatus != (int)ProjStatus.Attack){
				if(currentStatus != (int)ProjStatus.Jump){
					if(Math.Abs(Projectile.velocity.X) < Double.Epsilon){
						currentStatus = (int)ProjStatus.Idle;
					}else{
						currentStatus = (int)ProjStatus.Walk;
					}
				}

				if(Projectile.velocity.Y > Double.Epsilon){
					currentStatus = (int)ProjStatus.Fall;
				}

				if(Math.Abs(Projectile.velocity.Y) < Double.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
					Jump();
				}
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
            width = 16;
			height = 26;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
