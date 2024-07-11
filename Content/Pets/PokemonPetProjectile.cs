using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets
{
	public abstract class PokemonPetProjectile : ModProjectile
	{
		private int expGained = 0;
		public virtual int nAttackProjs => 1;
		public Projectile[] attackProjs;
		public int pokemonLvl;
		public virtual int baseDamage => 1;
		public virtual int PokemonBuff => 0;
		public virtual float enemySearchDistance => 10000;
		public virtual bool canAttackThroughWalls => false;
		public virtual int totalFrames => 0;
		public virtual int animationSpeed => 5;
		public virtual int[] idleStartEnd => [-1,-1];
		public virtual int[] walkStartEnd => [-1,-1];
		public virtual int[] jumpStartEnd => [-1,-1];
		public virtual int[] fallStartEnd => [-1,-1];
		public virtual int[] attackStartEnd => [-1,-1];

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
		public bool canAttack = false;
		public virtual int attackDuration => 0;
		public virtual int attackCooldown => 0;

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = totalFrames;
			Main.projPet[Projectile.type] = true;

			// Basics of CharacterPreviewAnimations explained in ExamplePetProjectile
			// Notice we define our own method to use in .WithCode() below. This technically allows us to animate the projectile manually using frameCounter and frame as well
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(walkStartEnd[0], walkStartEnd[1]-walkStartEnd[0]+1, animationSpeed)
				.WhenNotSelected(idleStartEnd[0], idleStartEnd[1]-idleStartEnd[0]+1)
				.WithOffset(-12f, 4f);
		}

        public override void OnSpawn(IEntitySource source)
        {
			attackProjs = new Projectile[nAttackProjs];
            base.OnSpawn(source);
        }

        public int GetExpGained(){
			int exp = expGained;
			expGained = 0;

			return exp;
		}

		public virtual int GetPokemonDamage(){
			int pokemonDamage = (int)(0.5f*baseDamage*(0.4f*pokemonLvl+2)+2);

			return pokemonDamage;
		}

		public void SetPokemonLvl(int lvl){
			pokemonLvl = lvl;
		}

        public override void AI() {
			Player player = Main.player[Projectile.owner];

			CheckActive(player);

			GetAllProjsExp();

			GeneralBehavior(player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(player, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			GetAllProjsExp();
			Visuals();
		}

		public virtual void GetAllProjsExp(){
			for(int i = 0; i < nAttackProjs; i++){
				GetProjExp(i);
			}
		}

		public void GetProjExp(int projIndex){
			int exp = 0;
			if(attackProjs[projIndex] != null){
				if(attackProjs[projIndex].active){
					PokemonAttack AttackProj = (PokemonAttack)attackProjs[projIndex].ModProjectile;
					exp = AttackProj.GetExpGained();
					if(exp != 0){
						CombatText.NewText(Projectile.Hitbox, new Color(255, 255, 255), "+"+exp+" Exp");
					}
				}else{
					attackProjs[projIndex] = null;
				}
			}
			expGained += exp;
		}

		public virtual void CheckActive(Player player) {
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff
			if (!player.dead && player.HasBuff(PokemonBuff)) {
				Projectile.timeLeft = 2;
			}
		}

		public virtual void GeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition) {
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

		public virtual void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter) {
			// Starting search distance
			distanceFromTarget = enemySearchDistance;
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

						if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall || canAttackThroughWalls)) {
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

		public virtual void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
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
			}
			else {
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

		public virtual void Jump(){
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

		public virtual void Visuals() {
			if(Math.Sign(Projectile.velocity.X) == 1 || Math.Sign(Projectile.velocity.X) == -1){
				Projectile.direction = Math.Sign(Projectile.velocity.X);
				Projectile.spriteDirection = Projectile.direction;
			}

			int initialFrame = 0;
			int finalFrame = 0;
			int frameSpeed = animationSpeed;

			switch(currentStatus){
				case (int)ProjStatus.Idle:
					initialFrame = idleStartEnd[0];
					finalFrame = idleStartEnd[1];
					break;
				case (int)ProjStatus.Walk:
					initialFrame = walkStartEnd[0];
					finalFrame = walkStartEnd[1];
					frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(Projectile.velocity.X), 2f, 100f));
					break;
				case (int)ProjStatus.Jump:
					initialFrame = jumpStartEnd[0];
					finalFrame = jumpStartEnd[1];
					break;
				case (int)ProjStatus.Fall:
					initialFrame = fallStartEnd[0];
					finalFrame = fallStartEnd[1];
					break;
				case (int)ProjStatus.Attack:
					initialFrame = attackStartEnd[0];
					finalFrame = attackStartEnd[1];
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
