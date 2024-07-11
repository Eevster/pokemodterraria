using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets.BulbasaurPet;
using Pokemod.Content.Pets.EeveePet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BulbasaurPetShiny
{
	public class BulbasaurPetProjectileShiny : PokemonPetProjectile
	{
		public override int nAttackProjs => 2;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<BulbasaurPetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 45;
		public override int attackCooldown => 0;

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,19];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 44;
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
				if(distanceFromTarget > 80f){
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
				if(distanceFromTarget < 140f){
					if(timer <= 0){
						if(canAttack && (attackProjs[0] == null || attackProjs[1] == null)){
							currentStatus = (int)ProjStatus.Attack;
							SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
							if(Projectile.owner == Main.myPlayer){
								if(attackProjs[0] == null) attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipBack>(), GetPokemonDamage(), 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X))];
								if(attackProjs[1] == null) attackProjs[1] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipFront>(), GetPokemonDamage(), 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X))];
							}
							timer = attackDuration;
							canAttack = false;
						}
					}
				}
				if(timer <= 0){
					if(!canAttack){
						if(currentStatus == (int)ProjStatus.Attack && attackCooldown != 0){
							currentStatus = (int)ProjStatus.Idle;
						}
						canAttack = true;
						timer = attackCooldown;
					}
				}
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] != null){
						if(attackProjs[i].active){
							Projectile.velocity.X *= 0.95f;
							attackProjs[i].Center = Projectile.Center;
						}else{
							attackProjs[i] = null;
						}
					}
				}
			}
			else {
				if(currentStatus == (int)ProjStatus.Attack){
					currentStatus = (int)ProjStatus.Idle;
					canAttack = true;
					timer = attackCooldown;
				}
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] != null){
						if(attackProjs[i].active){
							attackProjs[i].Kill();
						}
						attackProjs[i] = null;
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
            width = 24;
			height = 26;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
