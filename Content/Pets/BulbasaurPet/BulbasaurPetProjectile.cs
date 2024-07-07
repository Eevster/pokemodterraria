using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets.EeveePet;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BulbasaurPet
{
	public class BulbasaurPetProjectile : ModProjectile
	{
		public int currentStatus = 0;
		public enum ProjStatus
		{
			Idle,
			Walk,
			Jump,
			Fall,
			Attack
		}

		public int timer = 0;
		private int[] attackProjs = {-1,-1};
		public bool canAttack = true;
		public const int attackDuration = 45;
		public const int attackCooldown = 0;

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 20;
			Main.projPet[Projectile.type] = true;

			// Basics of CharacterPreviewAnimations explained in ExamplePetProjectile
			// Notice we define our own method to use in .WithCode() below. This technically allows us to animate the projectile manually using frameCounter and frame as well
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(6, 5, 5)
				.WhenNotSelected(0, 6)
				.WithOffset(-12f, 4f);
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 44;
			Projectile.height = 36;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0.3f;
			Projectile.tileCollide = true; 
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			CheckActive(player);

			GeneralBehavior(player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(player, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			Visuals();
		}

		private void CheckActive(Player player) {
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff
			if (!player.dead && player.HasBuff(ModContent.BuffType<BulbasaurPetBuff>())) {
				Projectile.timeLeft = 2;
			}
		}

		private void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) {
			Vector2 idlePosition = owner.Center;
			idlePosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

			float minionPositionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
			idlePosition.X += minionPositionOffsetX; // Go behind the player

			// All of this code below this line is adapted from Spazmamini code (ID 388, aiStyle 66)

			// Teleport to player if distance is too big
			vectorToIdlePosition = idlePosition - Projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();

			if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f) {
				// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
				// and then set netUpdate to true
				Projectile.position = idlePosition;
				Projectile.velocity *= 0.1f;
				Projectile.netUpdate = true;
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;

			// Fix overlap with other minions
			for (int i = 0; i < Main.maxProjectiles; i++) {
				Projectile other = Main.projectile[i];

				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width) {
					if (Projectile.position.X < other.position.X) {
						Projectile.velocity.X -= overlapVelocity;
					}
					else {
						Projectile.velocity.X += overlapVelocity;
					}

					if (Projectile.position.Y < other.position.Y) {
						Projectile.velocity.Y -= overlapVelocity;
					}
					else {
						Projectile.velocity.Y += overlapVelocity;
					}
				}
			}
		}

		private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
			// Starting search distance
			distanceFromTarget = 800f;
			targetCenter = Projectile.position;
			foundTarget = false;

			if (!foundTarget) {
				// This code is required either way, used for finding a target
				for (int i = 0; i < Main.maxNPCs; i++) {
					NPC npc = Main.npc[i];

					if (npc.CanBeChasedBy()) {
						float between = Vector2.Distance(npc.Center, Projectile.Center);
						bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
						bool inRange = between < distanceFromTarget;
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
						// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
						// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
						bool closeThroughWall = between < 100f;

						if(npc.boss){
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
							break;
						}

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall)) {
							distanceFromTarget = between;
							targetCenter = npc.Center;
							foundTarget = true;
						}
					}
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			Projectile.friendly = foundTarget;
		}

		private void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
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
						if(canAttack && (attackProjs[0] == -1 || attackProjs[1] == -1)){
							currentStatus = (int)ProjStatus.Attack;
							SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
							if(Projectile.owner == Main.myPlayer){
								if(attackProjs[0] == -1) attackProjs[0] = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipBack>(), 40, 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X));
								if(attackProjs[1] == -1) attackProjs[1] = Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipFront>(), 40, 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X));
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
				for(int i = 0; i < 2; i++){
					if(attackProjs[i] != -1){
						if(Main.projectile[attackProjs[i]].active){
							Projectile.velocity.X *= 0.95f;
							Main.projectile[attackProjs[i]].Center = Projectile.Center;
						}else{
							attackProjs[i] = -1;
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
				for(int i = 0; i < 2; i++){
					if(attackProjs[i] != -1){
						if(Main.projectile[attackProjs[i]].active){
							Main.projectile[attackProjs[i]].Kill();
						}
						attackProjs[i] = -1;
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

		private void Jump(){
			int scanHeight = 10;

			int moveDirection = 0;
			if (Projectile.velocity.X < 0f)
			{
				moveDirection = -1;
			}
			if (Projectile.velocity.X > 0f)
			{
				moveDirection = 1;
			}

			if(moveDirection == 0){
				return;
			}

			float jumpHeight = 0;

			for(int i = 1; i < 4; i++){
				jumpHeight = 0;
                for(int j = 0; j < scanHeight; j++){
					if(j == scanHeight-1){
						return;
					}
                    Vector2 scanPosition = Projectile.Bottom + new Vector2(moveDirection*16*i,-16*(j+1));

                    if(Collision.SolidCollision(scanPosition - new Vector2(8,16), 16, 16) || Main.tile[(int)scanPosition.X/16-moveDirection, (int)scanPosition.Y/16+1].IsHalfBlock || Main.tile[(int)scanPosition.X/16, (int)scanPosition.Y/16].IsHalfBlock){
                        jumpHeight += 1f;
                    }else{
						break;
					}
                }
				if(jumpHeight > 0){
					break;
				}
            }

			if(jumpHeight != 0){
				currentStatus = (int)ProjStatus.Jump;
				Projectile.velocity.Y -= (int)Math.Sqrt(2*0.3f*jumpHeight*16f);
			}
		}

		private void Visuals() {
			if(Math.Sign(Projectile.velocity.X) == 1 || Math.Sign(Projectile.velocity.X) == -1){
				Projectile.direction = Math.Sign(Projectile.velocity.X);
				Projectile.spriteDirection = Projectile.direction;
			}

			int initialFrame = 0;
			int finalFrame = 0;
			int frameSpeed = 5;

			switch(currentStatus){
				case (int)ProjStatus.Idle:
					initialFrame = 0;
					finalFrame = 5;
					break;
				case (int)ProjStatus.Walk:
					initialFrame = 6;
					finalFrame = 10;
					frameSpeed = (int)(5*3f/Math.Clamp(Math.Abs(Projectile.velocity.X), 2f, 100f));
					break;
				case (int)ProjStatus.Jump:
					initialFrame = 8;
					finalFrame = 8;
					break;
				case (int)ProjStatus.Fall:
					initialFrame = 9;
					finalFrame = 9;
					break;
				case (int)ProjStatus.Attack:
					initialFrame = 11;
					finalFrame = 19;
					break;
			}

			if(Projectile.frame > finalFrame || Projectile.frame < initialFrame){
				Projectile.frame = initialFrame;
			}

			Projectile.frameCounter++;

			if (Projectile.frameCounter >= frameSpeed) {
				Projectile.frameCounter = 0;
				Projectile.frame++;

				if (Projectile.frame > finalFrame) {
					Projectile.frame = initialFrame;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
			height = 26;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f) {
				Projectile.velocity.X = 0;
			}
			if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f) {
				Projectile.velocity.Y = 0;
			}

            return false;
        }
	}
}
