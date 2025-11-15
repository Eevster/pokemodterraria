using Pokemod.Content.Items;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Pokemod.Common.Players;
using Pokemod.Common.Systems;
using System.IO;
using Pokemod.Content.Dusts;
using Pokemod.Common.Configs;
using Pokemod.Content.Buffs;
using Terraria.Localization;

namespace Pokemod.Content.NPCs
{
	/// <summary>
	/// This file shows off a critter npc. The unique thing about critters is how you can catch them with a bug net.
	/// The important bits are: Main.npcCatchable, NPC.catchItem, and Item.makeNPC.
	/// We will also show off adding an item to an existing RecipeGroup (see ExampleRecipes.AddRecipeGroups).
	/// Additionally, this example shows an involved IL edit.
	/// </summary>
	public abstract class PokemonWildNPC : ModNPC
	{
		public virtual int pokemonID => 0;
		public virtual int minLevel => 2;
		public virtual int maxLevel => 100;
		public int lvl {
			get => (int)NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public virtual float catchRate => 45;
		public virtual string pokemonName => GetType().Name.Replace("CritterNPC","").Replace("Shiny","");
		public virtual bool shiny => GetType().Name.Contains("Shiny");
		public override string Texture => "Pokemod/Assets/Textures/Pokesprites/Pets/"+pokemonName+(shiny?"PetProjectileShiny":"PetProjectile");
		public virtual int totalFrames => 0;
		public virtual int hitboxWidth => 0;
		public virtual int hitboxHeight => 0;
		public virtual float moveSpeed => 1f;
		public virtual float maxJumpHeight => 7f;

		/// <summary>
		/// [baseHP, baseAtk, baseDef, baseSpatk, baseSpdef, baseSpeed]
		/// </summary>
		public virtual int[] baseStats => PokemonData.pokemonInfo[pokemonName].pokemonStats;
		public int[] finalStats = [0,0,0,0,0,0];

		public virtual string[] variants => [];
		public string variant;
		public virtual int variantProbDenominator => 10;

		public virtual int[][] spawnConditions =>
		[
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public int nature;
		public bool hostilePokemon;
		

		public override void SendExtraAI(BinaryWriter writer) {
			writer.Write(NPC.lifeMax);
			writer.Write(NPC.life);
			writer.Write(NPC.defense);
			writer.Write(finalStats[2]);
			writer.Write(finalStats[4]);
			writer.Write(finalStats[5]);
			writer.Write(moveDirection);
			writer.Write(flyDirection);
			writer.Write(isFlying);
			writer.Write(isSwimming);
		}

		public override void ReceiveExtraAI(BinaryReader reader) {
			NPC.lifeMax = reader.ReadInt32();
			NPC.life = reader.ReadInt32();
			NPC.defense = reader.ReadInt32();
			finalStats[2] = reader.ReadInt32();
			finalStats[4] = reader.ReadInt32();
			finalStats[5] = reader.ReadInt32();
			moveDirection = reader.ReadInt32();
			flyDirection = reader.ReadInt32();
			isFlying = reader.ReadBoolean();
			isSwimming = reader.ReadBoolean();
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = totalFrames;
			NPCID.Sets.CountsAsCritter[Type] = true;
			NPCID.Sets.TownCritter[Type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				// Influences how the NPC looks in the Bestiary
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults()
		{
			Asset<Texture2D> pokeTexture = ModContent.Request<Texture2D>(Texture);

			NPC.width = pokeTexture.Width();
			NPC.height = pokeTexture.Height();
			NPC.Hitbox = new Rectangle((int)(NPC.position.X + (NPC.width - hitboxWidth) / 2), (int)(NPC.position.Y + (NPC.height - hitboxHeight)), hitboxWidth, hitboxHeight);

			NPC.damage = 0;
			NPC.lifeMax = 100;
			NPC.defense = 0;

			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = -1;

			NPC.noTileCollide = !tangible;

			NPC.CanBeReplacedByOtherNPCs = true;

			if (shiny) NPC.rarity = 14;
		}

		public override void OnSpawn(IEntitySource source)
		{
			nature = 10 * Main.rand.Next(5) + Main.rand.Next(5);
			lvl = Main.rand.Next(minLevel, Math.Min(WorldLevel.MaxWorldLevel, maxLevel) + 1);
			//Probability of it being a variant
			if (Main.rand.NextBool(variantProbDenominator))
			{
				if (variants.Length > 0)
				{
					variant = variants[Main.rand.Next(variants.Length)];

					//Remove the possibility of spawning with the "Christmas" variant if it is no longer Christmas
					if (variant == "Christmas")
					{
						if (!Main.xMas)
						{
							variant = "";
						}
					}
					if (variant == "Halloween")
					{
						if (!Main.halloween)
						{
							variant = "";
						}
					}
				}
				else
				{
					variant = "";
				}
			}
			else
			{
				variant = "";
			}

			int[] IVs = PokemonNPCData.GenerateIVs();
			finalStats = PokemonNPCData.CalcAllStats(lvl, baseStats, IVs, [0, 0, 0, 0, 0, 0], nature);

			NPC.GetGlobalNPC<PokemonNPCData>().SetPokemonNPCData(pokemonName, shiny, lvl, baseStats, IVs, nature, variant: variant);

			/*if (Main.netMode == NetmodeID.Server)
			{
				ModPacket packet = Mod.GetPacket();
				packet.Write((byte)PokemodMessageType.SetNPCStats);
				packet.Write((byte)NPC.whoAmI);
				packet.Write(finalStats[0]);
				packet.Write(finalStats[2]);
				packet.Send();
			}*/

			NPC.lifeMax = finalStats[0];
			NPC.life = NPC.lifeMax;
			NPC.defense = Math.Max(finalStats[2], finalStats[4]);

			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
			}

			NPC.netUpdate = true;
		}
		
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Pokemod.NPCs." + pokemonName + "CritterNPC.Description")));
		}

        public override bool CanHitNPC(NPC target)
		{
			return false;
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void ModifyTypeName(ref string typeName)
        {
			typeName += " lvl " + lvl;
            base.ModifyTypeName(ref typeName);
        }

        public override void HitEffect(NPC.HitInfo hit) {
			if (NPC.life <= 0) {
				for (int i = 0; i < 6; i++) {
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Worm, 2 * hit.HitDirection, -2f);
					if (Main.rand.NextBool(2)) {
						dust.noGravity = true;
						dust.scale = 1.2f * NPC.scale;
					}
					else {
						dust.scale = 0.7f * NPC.scale;
					}
				}
			}
		}

        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            base.ModifyIncomingHit(ref modifiers);
			modifiers.FinalDamage *= 5;
        }

		public float GetSpawnChance(NPCSpawnInfo spawnInfo, float chance){
			if(shiny){
				chance *= 0.0024f;
				if(spawnInfo.Player.GetModPlayer<PokemonPlayer>().HasShinyCharm){
					chance *= 3f;
				}
			}

			if(Main.bloodMoon){
				chance *= 0.05f;
			}

			if(minLevel > WorldLevel.MaxWorldLevel){
				chance = 0;
			}

			if (spawnInfo.Player.HasBuff(ModContent.BuffType<PokemonRepel>()))
			{
				chance = 0;
			}

			return chance * ModContent.GetInstance<GameplayConfig>().PokemonSpawnMultiplier;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if(variant != null){
				if(variant != ""){
					if (ModContent.RequestIfExists<Texture2D>(Texture + "_" + variant, out Asset<Texture2D> variantTexture))
					{
						spriteBatch.Draw(variantTexture.Value, NPC.Center - screenPos,
							variantTexture.Frame(1, totalFrames, 0, (int)currentFrame), drawColor, NPC.rotation,
							variantTexture.Frame(1, totalFrames).Size() / 2f, NPC.scale, NPC.direction >= 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
						return false;
					}
				}
			}

            return true;
        }

        public virtual int animationSpeed => 6;
		public virtual int[] idleStartEnd => [-1,-1];
		public virtual int[] walkStartEnd => [-1,-1];
		public virtual int[] jumpStartEnd => [-1,-1];
		public virtual int[] fallStartEnd => [-1,-1];
		public virtual int[] attackStartEnd => [-1,-1];
		//Fly
		public virtual int[] idleFlyStartEnd => [-1,-1];
		public virtual int[] walkFlyStartEnd => [-1,-1];
		public virtual int[] attackFlyStartEnd => [-1,-1];
		//Swim
		public virtual int[] idleSwimStartEnd => [-1,-1];
		public virtual int[] walkSwimStartEnd => [-1,-1];
		public virtual int[] attackSwimStartEnd => [-1,-1];

		public virtual bool sideDiff => false;

		public virtual int moveStyle => 0;
		public virtual bool canSwim => false;
		public int moveDirection = 0;
		public int flyDirection = 0;
		public bool isSwimming = false;
		public bool isFlying = false;

		public virtual bool tangible => true;
		public virtual bool canRotate => false;

		public enum MovementStyle
		{
			Ground,
			Fly,
			Hybrid,
			Jump
		}

		private enum NPCStatus
		{
			Idle,
			Walk,
			Jump,
			Fall,
			Attack
		}

		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];
		public ref float currentFrame => ref NPC.ai[2];

		public override void AI()
		{
			NPC.chaseable = hostilePokemon;
			Movement();
			ExtraEffects();
			if (shiny) ShinyEffects();
		}

		// Here in FindFrame, we want to set the animation frame our npc will use depending on what it is doing.
		// We set npc.frame.Y to x * frameHeight where x is the xth frame in our spritesheet, counting from 0. For convenience, we have defined a enum above.
		public override void FindFrame(int frameHeight) {
			if(AI_State != (int)NPCStatus.Attack){
				if(Math.Abs(NPC.velocity.X) > float.Epsilon){
					NPC.direction = Math.Sign(NPC.velocity.X);
				}
			}
			NPC.spriteDirection = -NPC.direction;

			int initialFrame = 0;
			int finalFrame = 0;
			int frameSpeed = animationSpeed;

			if(isSwimming){
				switch(AI_State){
					case (int)NPCStatus.Idle:
						initialFrame = idleSwimStartEnd[0];
						finalFrame = idleSwimStartEnd[1];
						break;
					case (int)NPCStatus.Walk:
						initialFrame = walkSwimStartEnd[0];
						finalFrame = walkSwimStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(NPC.velocity.X/2), 2f, 10f));
						break;
					case (int)NPCStatus.Attack:
						initialFrame = attackSwimStartEnd[0];
						finalFrame = attackSwimStartEnd[1];
						break;
				}
			}else if(isFlying){
				switch(AI_State){
					case (int)NPCStatus.Idle:
						initialFrame = idleFlyStartEnd[0];
						finalFrame = idleFlyStartEnd[1];
						break;
					case (int)NPCStatus.Walk:
						initialFrame = walkFlyStartEnd[0];
						finalFrame = walkFlyStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(NPC.velocity.X/2), 2f, 10f));
						break;
					case (int)NPCStatus.Attack:
						initialFrame = attackFlyStartEnd[0];
						finalFrame = attackFlyStartEnd[1];
						break;
				}
			}else{
				switch(AI_State){
					case (int)NPCStatus.Idle:
						initialFrame = idleStartEnd[0];
						finalFrame = idleStartEnd[1];
						break;
					case (int)NPCStatus.Walk:
						initialFrame = walkStartEnd[0];
						finalFrame = walkStartEnd[1];
						frameSpeed = (int)(animationSpeed*3f/Math.Clamp(Math.Abs(NPC.velocity.X), 2f, 20f));
						break;
					case (int)NPCStatus.Jump:
						initialFrame = jumpStartEnd[0];
						finalFrame = jumpStartEnd[1];
						break;
					case (int)NPCStatus.Fall:
						initialFrame = fallStartEnd[0];
						finalFrame = fallStartEnd[1];
						break;
					case (int)NPCStatus.Attack:
						initialFrame = attackStartEnd[0];
						finalFrame = attackStartEnd[1];
						break;
				}
			}

			if(sideDiff && NPC.spriteDirection>0){
				initialFrame += totalFrames/2;
				finalFrame += totalFrames/2;
			}

			if(currentFrame > finalFrame || currentFrame < initialFrame){
				currentFrame = initialFrame;
			}

			NPC.frameCounter++;

			if (NPC.frameCounter >= frameSpeed) {
				NPC.frameCounter = 0;
				currentFrame++;

				if (currentFrame > finalFrame) {
					currentFrame = initialFrame;
				}
			}

			NPC.frame.Y = (int)currentFrame * frameHeight;
		}

		// Here, because we use custom AI (aiStyle not set to a suitable vanilla value), we should manually decide when Flutter Slime can fall through platforms
		public override bool? CanFallThroughPlatforms() {
			if (AI_State == (float)NPCStatus.Attack && NPC.HasValidTarget && Main.player[NPC.target].Top.Y > NPC.Bottom.Y) {
				// If Flutter Slime is currently falling, we want it to keep falling through platforms as long as it's above the player
				return true;
			}

			return false;
			// You could also return null here to apply vanilla behavior (which is the same as false for custom AI)
		}

		public virtual void ExtraEffects() {
		}

		public virtual void ShinyEffects() {
			if(Main.rand.NextBool(20)){
				int dustIndex = Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity = 0.01f*hitboxWidth*(Main.dust[dustIndex].position-NPC.Center).SafeNormalize(Vector2.UnitX);
			}
		}

		public virtual void Movement() {
			// Default movement parameters (here for attacking)
			float speed = moveSpeed;
			float speedMultiplier = 1f;
			
			if(moveStyle == (int)MovementStyle.Hybrid){
				if(flyDirection == 1 && NPC.velocity.Y == 0){
					isFlying = false;
				}
			}
			
			AI_Timer--;

			if(AI_Timer <= 0 && Main.netMode != NetmodeID.MultiplayerClient){
				AI_Timer = Main.rand.Next(60, 300);
				/*if(moveDirection == 0){
					moveDirection = 1+2*Main.rand.Next(-1,1);
				}else{
					moveDirection = 0;
				}*/
				moveDirection = Main.rand.Next(-1, 2);
				
				NPC.netUpdate = true;

				if(moveStyle == (int)MovementStyle.Hybrid){
					if(!isFlying){
						isFlying = Main.rand.NextBool(3);
					}
				}

				if(moveStyle == (int)MovementStyle.Hybrid || moveStyle == (int)MovementStyle.Fly){
					flyDirection = Main.rand.Next(-1,2);
					
					/*if(flyDirection != 0){
						NPC.velocity.Y = flyDirection;
					}*/
				}
				NPC.netUpdate = true;
			}

			if(moveStyle == (int)MovementStyle.Fly){
				isFlying = true;
			}

			if(canSwim){
				isSwimming = NPC.wet && !NPC.lavaWet && !NPC.honeyWet && !NPC.shimmerWet;
			}else{
				isSwimming = false;
			}
			
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.netUpdate = true;
			}

			if(isSwimming){
				speedMultiplier = 1.5f;
				float maxUpSpeed = -3;
				if(NPC.velocity.X == 0) maxUpSpeed = -2;
				if(NPC.velocity.Y > maxUpSpeed){
					NPC.velocity.Y -= 0.5f;
				}else{
					NPC.velocity.Y = maxUpSpeed;
				}
			} 
			if(isFlying) speedMultiplier = 2f;
			if(isSwimming || isFlying) NPC.noGravity = true;
			else NPC.noGravity = false;

			if (moveDirection != 0) {
				if(moveStyle != (int)MovementStyle.Jump){
					NPC.velocity.X = speed*speedMultiplier*moveDirection;
				}
			}else{
				NPC.velocity.X = 0;
			}
			if(isFlying){
				if (flyDirection != 0) {
					NPC.velocity.Y = speed*flyDirection;
				}else{
					NPC.velocity.Y = 0;
				}
			}

			if(isFlying || isSwimming){
				if(AI_State == (int)NPCStatus.Jump || AI_State == (int)NPCStatus.Fall){
					AI_State = (int)NPCStatus.Idle;
				}
				if(AI_State != (int)NPCStatus.Attack){
					if(Math.Abs(NPC.velocity.X) < 2){
						AI_State = (int)NPCStatus.Idle;
					}else{
						AI_State = (int)NPCStatus.Walk;
					}
				}
			}else{
				if(AI_State != (int)NPCStatus.Attack){
					if(AI_State != (int)NPCStatus.Jump){
						if(Math.Abs(NPC.velocity.X) < float.Epsilon){
							AI_State = (int)NPCStatus.Idle;
						}else{
							AI_State = (int)NPCStatus.Walk;
						}
					}

					if(NPC.velocity.Y > float.Epsilon){
						AI_State = (int)NPCStatus.Fall;
					}

					if(Math.Abs(NPC.velocity.Y) < float.Epsilon && !Collision.SolidCollision(NPC.Top-new Vector2(8,16), 16, 16)){
						Jump();
					}
				}
			}
			if (moveDirection != 0 && AI_State != (int)NPCStatus.Jump && AI_State != (int)NPCStatus.Attack) {
				if(moveStyle == (int)MovementStyle.Jump){
					if(Math.Abs(NPC.velocity.Y) < float.Epsilon && !Collision.SolidCollision(NPC.Top-new Vector2(8,16), 16, 16)){
						NPC.velocity.Y = -maxJumpHeight;
						NPC.velocity.X = speed*speedMultiplier*moveDirection;
					}
				}
			}

			if(canRotate){
				NPC.rotation -= NPC.spriteDirection*MathHelper.ToRadians(2f*NPC.velocity.Length());
			}
			
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.netUpdate = true;
			}
		}

		public virtual void Jump(){
			int direction = 0;
			if (NPC.velocity.X < 0f)
			{
				direction = -1;
			}
			if (NPC.velocity.X > 0f)
			{
				direction = 1;
			}

			if(direction == 0){
				return;
			}

			float jumpHeight = 0;

			for(int i = 1; i < 4; i++){
				jumpHeight = 0;
                for(int j = 0; j < maxJumpHeight; j++){
					if(j == maxJumpHeight-1){
						return;
					}
                    Vector2 scanPosition = NPC.Bottom + new Vector2(0,-2) + new Vector2(direction*16*i,-16*(j+1));

                    if(Collision.SolidCollision(scanPosition - new Vector2(8,16), 16, 16) || Main.tile[(int)scanPosition.X/16-direction, (int)scanPosition.Y/16+1].IsHalfBlock || Main.tile[(int)scanPosition.X/16, (int)scanPosition.Y/16].IsHalfBlock){
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
				AI_State = (int)NPCStatus.Jump;
				NPC.velocity.Y -= (int)Math.Sqrt(2*0.3f*jumpHeight*16f);
			}
		}
	}
}