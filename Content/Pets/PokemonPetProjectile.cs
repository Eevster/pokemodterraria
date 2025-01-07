using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Common.Players;
using Pokemod.Content.Items;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Pokemod.Content.DamageClasses;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Pokemod.Content.Pets
{
	public abstract class PokemonPetProjectile : ModProjectile
	{
		public override string Texture => "Pokemod/Assets/Textures/Pokesprites/Pets/"+GetType().Name;
		public int PokemonBuff = 0;
		private int expGained = 0;
		public int pokemonLvl;

		/// <summary>
		/// [baseHP, baseAtk, baseDef, baseSpatk, baseSpdef, baseSpeed]
		/// </summary>
		public int[] baseStats => PokemonNPCData.pokemonStats[GetType().Name.Replace("PetProjectile","").Replace("Shiny","")];
		public int[] IVs = [0,0,0,0,0,0];
		public int[] EVs = [0,0,0,0,0,0];
		public int[] finalStats = [0,0,0,0,0,0];

		public bool canBeHurt = true;
        public int hurtTime = 30;
        public int currentHp = 0;
		public string variant = "";
        public bool showHp;

		public virtual int nAttackProjs => 1;
		public Projectile[] attackProjs;
		public virtual float distanceToAttack => 800f;
		public virtual float enemySearchDistance => 1000;
		public virtual bool canAttackThroughWalls => false;

		public virtual float moveSpeed1 => 5f;
		public virtual float moveSpeed2 => 8f;
		public virtual float moveDistance1 => 400f;
		public virtual float moveDistance2 => 200f;
		public virtual bool canMoveWhileAttack => false;

		public virtual int hitboxWidth => 0;
		public virtual int hitboxHeight => 0;

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

		public virtual bool tangible => true;
		public virtual bool canRotate => false;

		public enum MovementStyle
		{
			Ground,
			Fly,
			Hybrid
		}

		public bool isEvolving = false;
		public int evolveTimer = 0;
		public const int maxEvolveTimer = 2*60;
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
			writer.Write((int)currentHp);
            writer.Write((double)currentStatus);
			writer.Write((double)expGained);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			currentHp = reader.ReadInt32();
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

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Asset<Texture2D> pokeTexture = ModContent.Request<Texture2D>(Texture);
			Projectile.width = hitboxWidth;
			DrawOffsetX = -(pokeTexture.Width() - Projectile.width)/2;
			Projectile.height = hitboxHeight;
			DrawOriginOffsetY = -(pokeTexture.Height()/(totalFrames)-hitboxHeight-4);
			Projectile.light = 0f;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.tileCollide = tangible;
			Projectile.ignoreWater = false;
		}

        public override void OnSpawn(IEntitySource source)
        {
			Player player = Main.player[Projectile.owner];
			PokemonBuff = ModContent.Find<ModBuff>("Pokemod", GetType().Name.Replace("Projectile","Buff")).Type;
			attackProjs = new Projectile[nAttackProjs];
			Projectile.Center += new Vector2(0,(player.height-Projectile.height)/2);
			currentHp = (int)Projectile.ai[0];
			if(currentHp == -1){
				currentHp = 10000;
			}else if(currentHp == 0){
				Projectile.Kill();
				return;
			}

			SoundEngine.PlaySound(new SoundStyle($"{nameof(Pokemod)}/Assets/Sounds/PKSpawn") with {Volume = 0.5f}, Projectile.Center);
			
            base.OnSpawn(source);
        }

		public void UpdateStats(){
            finalStats = PokemonNPCData.CalcAllStats(pokemonLvl, baseStats, IVs, EVs);
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

		public virtual int GetPokemonDamage(int power = 50, bool special = false){
			
			int atkStat = special?finalStats[3]:finalStats[1];
			int pokemonDamage = 2+(int)((2+2f*pokemonLvl/5)*power*atkStat/(50f*10f));
			pokemonDamage = (int)(pokemonDamage*Main.player[Projectile.owner].GetTotalDamage<PokemonDamageClass>().ApplyTo(1f));

			return pokemonDamage;
		}

		public void SetPokemonLvl(int lvl, int[] IVs = null, int[] EVs = null){
			if(pokemonLvl != 0 && pokemonLvl != lvl){
				CombatText.NewText(Projectile.Hitbox, new Color(255, 255, 255), Language.GetText("Mods.Pokemod.PokemonInfo.LevelUp").WithFormatArgs(GetType().Name.Replace("PetProjectileShiny","PetProjectile").Replace("PetProjectile",""), lvl).Value);
			}
			pokemonLvl = lvl;
			if(IVs != null) this.IVs = IVs;
			if(EVs != null) this.EVs = EVs;

			UpdateStats();
		}

		public virtual void SetEvolution(){
			if(canEvolve == -1){
				isEvolving = true;
				evolveTimer = maxEvolveTimer;
			}
		}

		public virtual void SetCanEvolve(){
			/*if(canEvolve == -1){
				if(levelEvolutionsNumber>0){
					if(pokemonLvl >= levelToEvolve){
						canEvolve = Main.rand.Next(0,levelEvolutionsNumber);
					}
				}
			}*/
			if(canEvolve == -1){
				if(levelEvolutionsNumber>0){
					if(pokemonLvl >= levelToEvolve){
						SetEvolution();
						canEvolve = Main.rand.Next(0,levelEvolutionsNumber);
					}
				}
			}
		}

		public virtual bool UseEvoItem(string itemName){
			if(itemToEvolve.Length>0){
				for(int i = 0; i < itemToEvolve.Length; i++){
					if(itemName == itemToEvolve[i]){
						SetEvolution();
						canEvolve = levelEvolutionsNumber+i;
						itemEvolve = true;
						return true;
					}
				}
			}
			return false;
		}

		public virtual void EvolutionProcess(){
			if(isEvolving){
				if(evolveTimer>0){
					evolveTimer--;
				}
			}
		}

		public virtual string GetCanEvolve(){
			if(canEvolve != -1){
				if(isEvolving && evolveTimer <= 0){
					return evolutions[canEvolve];
				}
			}
			return "";
		}

        public override void AI() {
			Player player = Main.player[Projectile.owner];

			CheckActive(player);

			setMaxHP();
			hurtTimer();
            GetAllProjsExp();
            TakeDamage();

			GeneralBehavior(player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
			SearchForTargets(player, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
			Movement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);
			GetAllProjsExp();
			EvolutionProcess();
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

		public void setMaxHP()
        {
			if(pokemonLvl != 0 && finalStats[0] != 0){
				if(currentHp > finalStats?[0]){
					currentHp = finalStats[0];
				}
			}
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
			idlePosition.Y -= 16-(owner.height-Projectile.height)/2; // Go up 48 coordinates (three tiles from the center of the player)

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
					if(Main.netMode != NetmodeID.SinglePlayer){
						float sqrMaxDetectDistance = distanceFromTarget*distanceFromTarget;
						for (int k = 0; k < Main.maxPlayers; k++) {
							if(Main.player[k] != null){
								Player target = Main.player[k];
								if(target.whoAmI != Projectile.owner){
									if(target.active && !target.dead){
										float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);
										bool lineOfSight = Collision.CanHitLine(Projectile.Center-Vector2.One, 2, 2, target.position, target.width, target.height);
										bool closeThroughWall = Vector2.Distance(target.Center, Projectile.Center) < 100f || canAttackThroughWalls;

										// Check if it is within the radius
										if (sqrDistanceToTarget < sqrMaxDetectDistance && (lineOfSight || closeThroughWall)) {
											if(target.hostile){
												sqrMaxDetectDistance = sqrDistanceToTarget;
												distanceFromTarget = Vector2.Distance(target.Center, Projectile.Center);
												targetCenter = target.Center;
												foundTarget = true;
											}
										}
									}
								}
							}
						}
					}

					for (int i = 0; i < Main.maxNPCs; i++) {
						NPC npc = Main.npc[i];

						if (npc.CanBeChasedBy()) {
							float between = Vector2.Distance(npc.Center, Projectile.Center);
							bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
							bool inRange = between < distanceFromTarget;
							bool lineOfSight = Collision.CanHitLine(Projectile.Center-Vector2.One, 2, 2, npc.position, npc.width, npc.height);
							// Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
							// The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
							bool closeThroughWall = between < 100f;

							if (inRange && (closest || !foundTarget) && (lineOfSight || closeThroughWall || canAttackThroughWalls) && !(npc.GetGlobalNPC<PokemonNPCData>().isPokemon && npc.GetGlobalNPC<PokemonNPCData>().shiny)) {
								if(npc.boss){
									distanceFromTarget = between;
									targetCenter = npc.Center;
									foundTarget = true;
									break;
								}
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

				if(currentStatus == (int)ProjStatus.Attack){
					Projectile.direction = Math.Sign(targetCenter.X-Projectile.Center.X);
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
				if(isFlying && distanceToIdlePosition > 1200f && tangible){
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

					if(distanceToIdlePosition < 60f && tangible){
						Projectile.tileCollide = true;
						if(moveStyle == (int)MovementStyle.Hybrid){
							isFlying = false;
						}
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

			if(canRotate){
				Projectile.rotation += Projectile.spriteDirection*MathHelper.ToRadians(1.5f*Projectile.velocity.Length());
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
                    Vector2 scanPosition = Projectile.Center + new Vector2(0,hitboxHeight/2) + new Vector2(moveDirection*16*i,-16*(j+1));

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
			if(currentStatus != (int)ProjStatus.Attack){
				if(Math.Abs(Projectile.velocity.X) > float.Epsilon){
					Projectile.direction = Math.Sign(Projectile.velocity.X);
				}
			}
			Projectile.spriteDirection = Projectile.direction;

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

		public int calIncomingDmg(int npcdmg){
            //calling Hp
            if(currentHp > finalStats[0]) { currentHp = finalStats[0]; }
            //cal damage versus defense for pokemon
			//int dmg = npcdmg - finalStats[2]/2;
			int dmg = 2 + (int)(Math.Clamp(npcdmg-2f,0f,9999f)/finalStats[2]);

            //if dmg is less than 1 deal at least 1
            if (dmg <= 0) dmg = 1;
            //hurt pokemon
            currentHp -= dmg;
            //display damage done
           // Main.NewText("HP: " + currentHp + "/" + getMaxHP());
            CombatText.NewText(Projectile.Hitbox, new Color(255, 50, 50), dmg);
            //if health is less than or equal to 0 despawn pokemon
            if (currentHp <= 0) {
				currentHp = 0;
				Main.player[Projectile.owner].ClearBuff(PokemonBuff);
				if(Projectile.owner == Main.myPlayer){
					Main.NewText(Language.GetTextValue("Mods.Pokemod.PokemonInfo.FaintedMsg"), 255, 130, 130); 
				}
			}
            
            return dmg;
        }

		public void manualDmg(int dmg){
			if (dmg <= 0) dmg = 1;

            currentHp -= dmg;
            CombatText.NewText(Projectile.Hitbox, new Color(255, 50, 50), dmg);

            if (currentHp <= 0) {
				currentHp = 0;
				Main.player[Projectile.owner].ClearBuff(PokemonBuff);
				if(Projectile.owner == Main.myPlayer){
					Main.NewText(Language.GetTextValue("Mods.Pokemod.PokemonInfo.FaintedMsg"), 255, 130, 130); 
				}
			}
		}
        
        public void regenHP(int amount, bool showText = true){
            // heal hp
            currentHp += amount;
            if(showText) CombatText.NewText(Projectile.Hitbox, new Color(50, 255, 50), "+" + amount);
			if(currentHp > finalStats[0]) { currentHp = finalStats[0]; }
        }
        
        public void TakeDamage(){
            NPC npc = null;
            for (int i = 0; i < Main.maxNPCs; i++){
                npc = Main.npc[i];

                if (npc.CanBeChasedBy() && npc.damage != 0){
                    if (Projectile.Hitbox.Intersects(npc.getRect()) && !canBeHurt){
                        int npcdmg = npc.defDamage;
                        if(currentHp != 0){
							calIncomingDmg(npcdmg);
						}
                        canBeHurt = true;
                    }
                }
            }
        }
      
        public void hurtTimer(){
            if (canBeHurt){
                hurtTime--;

                if (hurtTime <= 0){
                    hurtTime = 30;
                    canBeHurt = false;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if(variant != null){
				if(variant != ""){
					Asset<Texture2D> variantTexture = ModContent.Request<Texture2D>(Texture+"_"+variant);

                    Main.EntitySpriteDraw(variantTexture.Value, Projectile.Center - Main.screenPosition,
                        variantTexture.Frame(1, totalFrames, 0, Projectile.frame), lightColor, Projectile.rotation,
                        variantTexture.Frame(1, totalFrames).Size() / 2f, Projectile.scale, Projectile.spriteDirection>=0?SpriteEffects.None:SpriteEffects.FlipHorizontally, 0);

					return false;
                }
            }
            
            return true;
        }

        public override bool PreDrawExtras()
        {
            if(finalStats[0] != 0){
				Asset<Texture2D> barTexture = ModContent.Request<Texture2D>("Pokemod/Assets/Textures/PlayerVisuals/PokemonHPBar");

				float quotient = (float)currentHp/ finalStats[0];// Creating a quotient that represents the difference of your currentResource vs your maximumResource, resulting in a float of 0-1f.
				quotient = Utils.Clamp(quotient, 0f, 1f); // Clamping it to 0-1f so it doesn't go over that.

				if (currentHp < finalStats[0])
				{
					// Now, using this hitbox, we draw a gradient by drawing vertical lines while slowly interpolating between the 2 colors.
					int left = -24;
					int right = 24;
					int steps = (int)((right - left) * quotient);

					for (int i = 0; i < steps; i += 1)
					{
						float percent = (float)i / (right - left);
						Main.EntitySpriteDraw(TextureAssets.MagicPixel.Value, Entity.Top + new Vector2(left + i, -20) - Main.screenPosition, new Rectangle(0, 0, 1, 8), Color.Lerp(new Color(50, 255, 50), new Color(50, 255, 50), percent), 0, new Rectangle(0, 0, 1, 8).Size() * 0.5f, 1, SpriteEffects.None, 0);

					}
					Main.EntitySpriteDraw(barTexture.Value, Entity.Top + new Vector2(0,-20) - Main.screenPosition, barTexture.Value.Bounds, Color.White, 0, barTexture.Size() * 0.5f, 1, SpriteEffects.None, 0);
				}
			}
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
			if(isEvolving && evolveTimer > 0){
				Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
				float lightScale = (float)(0.1f*Math.Sqrt(maxEvolveTimer-evolveTimer));
				for(int i = 0; i < 10; i++){
					DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2, new Color(255, 255, 255) * 0.5f, new Color(255, 255, 255), 0.5f, 0f, 0.5f, 0.5f, 1f, Projectile.rotation+ MathHelper.ToRadians(i*360f/10f) + MathHelper.ToRadians(Main.rand.Next(-8,9)), new Vector2(2f, Utils.Remap(0.5f, 0f, 1f, 4f, 1f)) * Projectile.scale * lightScale, 2*Vector2.One * Projectile.scale * lightScale);
				}
			}
        }

        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness) {
			Texture2D sparkleTexture = TextureAssets.Extra[98].Value;
			Color bigColor = shineColor * opacity * 0.5f;
			bigColor.A = 0;
			Vector2 origin = sparkleTexture.Size() / 2f;
			Color smallColor = drawColor * 0.5f;
			float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);
			Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;
			Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;
			bigColor *= lerpValue;
			smallColor *= lerpValue;
			// Bright, large part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);
			// Dim, small part
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);
			Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
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
				if(!(canEvolve != -1 && isEvolving && evolveTimer <= 0)){
					Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<DespawnPokemon>(), 0, 0, Projectile.owner);
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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
