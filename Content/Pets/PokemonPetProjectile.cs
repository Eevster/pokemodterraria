using System;
using System.IO;
using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
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
		public virtual float distanceToAttack => 800f;
		public int pokemonLvl;
		public virtual int baseDamage => 1;
		public virtual int PokemonBuff => 0;
		public virtual float enemySearchDistance => 1000;
		public virtual bool canAttackThroughWalls => false;

		public virtual float moveSpeed1 => 5f;
		public virtual float moveSpeed2 => 8f;
		public virtual float moveDistance1 => 400f;
		public virtual float moveDistance2 => 200f;
		public virtual bool canMoveWhileAttack => false;

		public virtual int totalFrames => 0;
		public virtual int animationSpeed => 5;
		public virtual int[] idleStartEnd => [-1,-1];
		public virtual int[] walkStartEnd => [-1,-1];
		public virtual int[] jumpStartEnd => [-1,-1];
		public virtual int[] fallStartEnd => [-1,-1];
		public virtual int[] attackStartEnd => [-1,-1];
		public virtual int maxJumpHeight => 10;
		//Fly
		public virtual int[] idleFlyStartEnd => [-1,-1];
		public virtual int[] walkFlyStartEnd => [-1,-1];
		public virtual int[] attackFlyStartEnd => [-1,-1];
		//Swim
		public virtual int[] idleSwimStartEnd => [-1,-1];
		public virtual int[] walkSwimStartEnd => [-1,-1];
		public virtual int[] attackSwimStartEnd => [-1,-1];

		public bool canAttackOutTimer = false;
		public virtual int moveStyle => 0;
		public virtual bool canSwim => false;
		public bool isSwimming = false;
		public bool isFlying = false;

		public enum MovementStyle
		{
			Ground,
			Fly,
			Hybrid
		}

		public int canEvolve = -1;
		public bool itemEvolve = false;
		public virtual string[] evolutions => [];
		public virtual int levelToEvolve => -1;
		public virtual int levelEvolutionsNumber => 0;
		public virtual string[] itemToEvolve => [];

		public int currentStatus = 0;
		public enum ProjStatus
		{
			Idle,
			Walk,
			Jump,
			Fall,
			Attack
		}

		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write((double)currentStatus);
			writer.Write((double)expGained);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            currentStatus = (int)reader.ReadDouble();
			expGained = (int)reader.ReadDouble();
            base.ReceiveExtraAI(reader);
        }

		public int timer = 0;
		public bool canAttack = false;
		public virtual int attackDuration => 0;
		public virtual int attackCooldown => 0;

		//Item effects
		public bool rareCandy = false;

		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = totalFrames;
			Main.projPet[Projectile.type] = true;

			// Basics of CharacterPreviewAnimations explained in ExamplePetProjectile
			// Notice we define our own method to use in .WithCode() below. This technically allows us to animate the projectile manually using frameCounter and frame as well
			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(walkStartEnd[0], walkStartEnd[1]-walkStartEnd[0]+1, animationSpeed)
				.WhenNotSelected(idleStartEnd[0], idleStartEnd[1]-idleStartEnd[0]+1)
				.WithOffset(0f, 4f);
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

		public bool GetRareCandy(){
			bool used = rareCandy;
			rareCandy = false;

			return used;
		}

		public virtual int GetPokemonDamage(){
			int pokemonDamage = (int)(0.6f*baseDamage*(0.4f*pokemonLvl+2)+2);

			return pokemonDamage;
		}

		public void SetPokemonLvl(int lvl){
			if(pokemonLvl != 0 && pokemonLvl != lvl){
				CombatText.NewText(Projectile.Hitbox, new Color(255, 255, 255), GetType().Name.Replace("PetProjectileShiny","PetProjectile").Replace("PetProjectile","")+" grew to lvl "+lvl);
			}
			pokemonLvl = lvl;
		}

		public virtual void SetCanEvolve(){
			if(canEvolve == -1){
				if(levelEvolutionsNumber>0){
					if(pokemonLvl >= levelToEvolve){
						canEvolve = Main.rand.Next(0,levelEvolutionsNumber);
					}
				}
			}
		}

		public virtual bool UseEvoItem(string itemName){
			if(itemToEvolve.Length>0){
				for(int i = 0; i < itemToEvolve.Length; i++){
					if(itemName == itemToEvolve[i]){
						canEvolve = levelEvolutionsNumber+i;
						itemEvolve = true;
						return true;
					}
				}
			}
			return false;
		}

		public virtual string GetCanEvolve(){
			if(canEvolve != -1){
				return evolutions[canEvolve];
			}
			return "";
		}

        public override void AI() {
			Player player = Main.player[Projectile.owner];

			CheckActive(player);

			GetAllProjsExp();

			GeneralBehavior(player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(player, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			GetAllProjsExp();
			SetCanEvolve();
			Visuals();

			if(Main.myPlayer == Projectile.owner){
				Projectile.netUpdate = true;
			}
		}

		public virtual void GetAllProjsExp(){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					GetProjExp(i);
				}
			}
		}

		public void GetProjExp(int projIndex){
			int exp = 0;
			if(attackProjs[projIndex] != null){
				if(attackProjs[projIndex].active){
					if(attackProjs[projIndex].ModProjectile is PokemonAttack){
						PokemonAttack AttackProj = (PokemonAttack)attackProjs[projIndex]?.ModProjectile;
						if(AttackProj != null){
							AttackProj.pokemonProj = Projectile;
							/*exp = AttackProj.GetExpGained();
							if(exp != 0){
								CombatText.NewText(Projectile.Hitbox, new Color(255, 255, 255), "+"+exp+" Exp");
							}*/
						}
					}
				}else{
					attackProjs[projIndex] = null;
				}
			}
			expGained += exp;
		}

		public void SetExtraExp(int extraExp){
			if(Projectile.owner == Main.myPlayer){
				if(extraExp > 0){
					CombatText.NewText(Projectile.Hitbox, new Color(255, 255, 255), "+"+extraExp+" Exp");
				}
				Projectile.netUpdate = true;
			}

			expGained += extraExp;
		}

		public virtual void CheckActive(Player player) {
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff
			if (!player.dead && player.HasBuff(PokemonBuff)) {
				Projectile.timeLeft = 2;
				player.AddBuff(PokemonBuff, 10);
			}
			if(Main.myPlayer == Projectile.owner){
				Projectile.netUpdate = true;
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

			PokemonPlayer trainer = owner.GetModPlayer<PokemonPlayer>();

			if(trainer.attackMode == (int)PokemonPlayer.AttackMode.Auto_Attack){
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
			}else if(trainer.attackMode == (int)PokemonPlayer.AttackMode.Directed_Attack){
				targetCenter = trainer.attackPosition;
				distanceFromTarget = Vector2.Distance(Projectile.Center, targetCenter);
				foundTarget = true;
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			Projectile.friendly = foundTarget;
		}

		public virtual void Movement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition) {
			// Default movement parameters (here for attacking)
			float speed = moveSpeed1;
			float inertia = 20f;
			float speedMultiplier = 1f;

			float maxFallSpeed = 10f;

			if(moveStyle == (int)MovementStyle.Fly){
				isFlying = true;
			}

			if(canSwim){
				isSwimming = Projectile.wet && !Projectile.lavaWet && !Projectile.honeyWet && !Projectile.shimmerWet;
			}else{
				isSwimming = false;
			}

			if(isSwimming) speedMultiplier = 1.5f;

			if (foundTarget) {
				if((currentStatus != (int)ProjStatus.Attack && !canMoveWhileAttack) || canMoveWhileAttack){
					if(distanceFromTarget > moveDistance1){
						Vector2 direction = targetCenter - Projectile.Center;
						direction.Normalize();
						direction *= speedMultiplier*speed;

						if(isFlying || isSwimming){
							Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
						}else{
							Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + direction) / inertia).X;

							if((targetCenter - Projectile.Center).Y < 0 && -(targetCenter - Projectile.Center).Y > Math.Abs((targetCenter - Projectile.Center).X)){
								if(Math.Abs(Projectile.velocity.Y) < float.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
									currentStatus = (int)ProjStatus.Jump;
									Projectile.velocity.Y -= (int)Math.Sqrt(2*0.3f*Math.Clamp(Math.Abs((targetCenter - Projectile.Center).Y),0,160));
								}
							}
						}
					}else{
						if(distanceFromTarget > moveDistance2){
							Vector2 direction = targetCenter - Projectile.Center;
							direction.Normalize();
							direction *= speedMultiplier*speed/2;

							if(isFlying || isSwimming){
								Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
							}else{
								Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + direction) / inertia).X;
							}
						}else{
							Projectile.velocity.X *= 0.95f;
							if(distanceFromTarget < 100f && moveStyle == (int)MovementStyle.Hybrid){
								isFlying = false;
							}
						}
					}
				}else{
					Projectile.velocity.X *= 0.9f;
				}

				if(distanceFromTarget < distanceToAttack){
					if(timer <= 0){
						if(canAttack){
							Attack(distanceFromTarget, targetCenter);
						}
					}
				}else{
					if(moveStyle == (int)MovementStyle.Hybrid){
						isFlying = true;
					}
				}

				if(canAttackOutTimer){
					AttackOutTimer(distanceFromTarget, targetCenter);
				}

				if(Projectile.owner == Main.myPlayer){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] != null){
							if(attackProjs[i].active){
								UpdateAttackProjs(i, ref maxFallSpeed);
							}else{
								attackProjs[i] = null;
							}
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
			}
			else {
				if(timer <= 0){
					if(!canAttack){
						if(currentStatus == (int)ProjStatus.Attack){
							currentStatus = (int)ProjStatus.Idle;
						}
						canAttack = true;
						timer = attackCooldown;
					}
				}
				if(Projectile.owner == Main.myPlayer){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] != null){
							if(attackProjs[i].active){
								UpdateNoAttackProjs(i);
							}else{
								attackProjs[i] = null;
							}
						}
					}
				}
				if(isFlying && distanceToIdlePosition > 1200f && moveStyle == (int)MovementStyle.Hybrid){
					Projectile.tileCollide = false;
				}
				// Minion doesn't have a target: return to player and idle
				if (distanceToIdlePosition > 600f) {
					// Speed up the minion if it's away from the player
					if(moveStyle == (int)MovementStyle.Hybrid){
						isFlying = true;
					}
					speed = moveSpeed2;
				}
				else {
					// Slow down the minion if closer to the player
					speed = moveSpeed1;

					if(distanceToIdlePosition < 60f && moveStyle == (int)MovementStyle.Hybrid){
						Projectile.tileCollide = true;
						isFlying = false;
					}
				}

				if (distanceToIdlePosition > 80f) {
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToIdlePosition.Normalize();
					vectorToIdlePosition *= speedMultiplier*speed;
					if(isFlying || isSwimming){
						Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
					}else{
						Projectile.velocity.X = ((Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia).X;
					}
				}else{
					if(Math.Abs(Projectile.velocity.X)>0.2f){
						Projectile.velocity.X *= 0.9f;
					}else{
						Projectile.velocity.X = 0;
					}
				}
			}

			if(isFlying || isSwimming){
				if(currentStatus == (int)ProjStatus.Jump || currentStatus == (int)ProjStatus.Fall){
					currentStatus = (int)ProjStatus.Idle;
				}
				if(currentStatus != (int)ProjStatus.Attack){
					if(Math.Abs(Projectile.velocity.X) < 3){
						currentStatus = (int)ProjStatus.Idle;
					}else{
						currentStatus = (int)ProjStatus.Walk;
					}
				}
			}else{
				if(currentStatus != (int)ProjStatus.Attack){
					if(currentStatus != (int)ProjStatus.Jump){
						if(Math.Abs(Projectile.velocity.X) < float.Epsilon){
							currentStatus = (int)ProjStatus.Idle;
						}else{
							currentStatus = (int)ProjStatus.Walk;
						}
					}

					if(Projectile.velocity.Y > float.Epsilon){
						currentStatus = (int)ProjStatus.Fall;
					}

					if(Math.Abs(Projectile.velocity.Y) < float.Epsilon && !Collision.SolidCollision(Projectile.Top-new Vector2(8,16), 16, 16)){
						Jump();
					}
				}

				Projectile.velocity.Y += 0.2f;
				if(Projectile.velocity.Y > maxFallSpeed){
					Projectile.velocity.Y = maxFallSpeed;
				}
			}

			if(timer > 0){
				timer--;
			}
		}

		public virtual void Attack(float distanceFromTarget, Vector2 targetCenter){

		}

		public virtual void AttackOutTimer(float distanceFromTarget, Vector2 targetCenter){

		}

		public virtual void UpdateAttackProjs(int i, ref float maxFallSpeed){

		}

		public virtual void UpdateNoAttackProjs(int i){

		}

		public virtual void Jump(){
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
                for(int j = 0; j < maxJumpHeight; j++){
					if(j == maxJumpHeight-1){
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

			if(isSwimming){
				switch(currentStatus){
					case (int)ProjStatus.Idle:
						initialFrame = idleSwimStartEnd[0];
						finalFrame = idleSwimStartEnd[1];
						break;
					case (int)ProjStatus.Walk:
						initialFrame = walkSwimStartEnd[0];
						finalFrame = walkSwimStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(Projectile.velocity.X/2), 2f, 10f));
						break;
					case (int)ProjStatus.Attack:
						initialFrame = attackSwimStartEnd[0];
						finalFrame = attackSwimStartEnd[1];
						break;
				}
			}else if(isFlying){
				switch(currentStatus){
					case (int)ProjStatus.Idle:
						initialFrame = idleFlyStartEnd[0];
						finalFrame = idleFlyStartEnd[1];
						break;
					case (int)ProjStatus.Walk:
						initialFrame = walkFlyStartEnd[0];
						finalFrame = walkFlyStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(Projectile.velocity.X/2), 2f, 10f));
						break;
					case (int)ProjStatus.Attack:
						initialFrame = attackFlyStartEnd[0];
						finalFrame = attackFlyStartEnd[1];
						break;
				}
			}else{
				switch(currentStatus){
					case (int)ProjStatus.Idle:
						initialFrame = idleStartEnd[0];
						finalFrame = idleStartEnd[1];
						break;
					case (int)ProjStatus.Walk:
						initialFrame = walkStartEnd[0];
						finalFrame = walkStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(Projectile.velocity.X), 2f, 20f));
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

        public override void OnKill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] != null){
						if(attackProjs[i].active){
							attackProjs[i].Kill();
						}else{
							attackProjs[i] = null;
						}
					}
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
