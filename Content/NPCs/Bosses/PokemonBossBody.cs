using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.NPCs.Bosses.Mewtwo;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.NPCs.Bosses
{
	[AutoloadBossHead] // This attribute looks for a texture called "ClassName_Head_Boss" and automatically registers it as the NPC boss head icon
	public abstract class PokemonBossBody : ModNPC
	{
        public virtual int attackType => (int)TypeIndex.Normal; //The attack type of the boss's collision damage.
        public virtual int defenceType1 => (int)TypeIndex.Normal;
        public virtual int defenceType2 => (int)TypeIndex.Normal;

        public float accelerationPower = 0.5f;
		public float maxVelocity = 20f;
		public float drag = 0.95f;
		public bool bodyVisible = true;

		public Asset<Texture2D> auraTexture = null;
		public int auraTotalFrames = 1;
		public int auraTicksPerFrame = 4;
		public int auraSingleFrame = -1;
        public bool drawAura = false;
		public Color auraColor = default;

		public BossState neutralState;
        public BossState bossState;
        public int lastActiveState;

        // This boss has a second phase and we want to give it a second boss head icon, this variable keeps track of the registered texture from Load().
        // It is applied in the BossHeadSlot hook when the boss is in its second Phase
        public Player targetPlayer;

		// Using NPC.ai[] for values that need to be synced in multiplayer.

		public int CurrentFrame {
			get => (int)NPC.ai[0];
			set => NPC.ai[0] = value;
		}

		public Vector2 MovementDestination {
			get => new Vector2(NPC.ai[1], NPC.ai[2]);
			set {
				NPC.ai[1] = value.X;
				NPC.ai[2] = value.Y;
			}
		}
		public Vector2 LastMovementDestination { get; set; } = Vector2.Zero;
		public ref float StateTimer => ref NPC.localAI[0];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
            });
        }



		/*
		public override void BossHeadSlot(ref int index) {
			int slot = secondPhaseHeadSlot;
			if (StateIndex > 9 && slot != -1) {
				// If the boss is in its second Phase, display the other head icon instead
				index = slot;
			}
		}
		*/

		public override void SetDefaults() {
			//Standard Stats for all Custom Bosses
            NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.SpawnWithHigherTime(30);
			NPC.boss = true;
			NPC.npcSlots = 8f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
			NPC.aiStyle = -1;
			NPCID.Sets.MPAllowedEnemies[Type] = true;
        }

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public Player FindTarget()
		{
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
			return player;
        }

		public void BossRoar()
		{
            SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

            // This adds a screen shake (screenshake) similar to Deerclops
            PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
            Main.instance.CameraModifiers.Add(modifier);
        }

        public override bool PreKill()
        {
            bossState.ClearState();
            return base.PreKill();
        }

		public override void AI() {
			// This should almost always be the first code in AI() as it is responsible for finding the proper targetplayer target
			targetPlayer = FindTarget();

			if (targetPlayer.dead) {
				// If the targeted targetplayer is dead, flee
				NPC.velocity.Y -= 4f;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                bossState.ClearState();
                NPC.EncourageDespawn(10);
				return;
			}
			float horizontalDistance = (targetPlayer.Center - NPC.Center).X;
			float horizontalSpeed = NPC.velocity.X;

            if (Math.Abs(horizontalSpeed) > 8)
			{
                NPC.direction = Math.Sign(horizontalSpeed);
            }
			else if (Math.Abs(horizontalDistance) > 5)
            {
				NPC.direction = Math.Sign(horizontalDistance);
            }

            NPC.velocity *= drag;
            bossState.Run();
        }

        public override void FindFrame(int frameHeight)
        {
			if (StateTimer == 0) //First frame
			{
				CurrentFrame = bossState.frameStart;
			}
			else if (StateTimer % bossState.ticksPerFrame == 0) //Next frame
			{
                bool Incrementing = bossState.frameStart <= bossState.frameEnd;
				CurrentFrame += Incrementing ? 1 : -1;
				if (!(Incrementing ^ CurrentFrame > bossState.frameEnd)) //Restart after frameEnd (accounting for direction)
				{
					CurrentFrame = bossState.frameStart;
				}
            }
            NPC.frame.Y = CurrentFrame * frameHeight;
        }

		//Draws an animation over the top of the boss, useful for glowing effects that are attached.
		public void SetAura(Asset<Texture2D> texture, int totalFrames = 4, int ticksPerFrame = 4, int singleFrame = -1, bool active = true, Color color = default)
		{
			auraTexture = texture;
			auraTotalFrames = totalFrames;
			auraTicksPerFrame = ticksPerFrame;
			auraSingleFrame = singleFrame;
			drawAura = active;
			auraColor = color;
		}

		public void DustBurst(int dustID, int count, int force, float scale = 1f, Vector2 position = default)
		{
			if (position == default)
			{
				position = NPC.Center;
			}
            for (int i = 0; i < count; i++)
            {
				Vector2 direction = Main.rand.NextVector2Unit(0, MathHelper.TwoPi);
                int dust = Dust.NewDust(position, 0, 0, dustID, direction.X * force, direction.Y * force, default, default, scale);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if (!bodyVisible) return false;
			Asset<Texture2D> texture = ModContent.Request<Texture2D>(NPC.ModNPC.Texture);

            Main.EntitySpriteDraw(
                texture.Value,
                NPC.Center - Main.screenPosition,
                texture.Frame(1, Main.npcFrameCount[Type],
                0,
                CurrentFrame),
                drawColor,
                NPC.rotation,
                texture.Frame(1, Main.npcFrameCount[Type]).Size() / 2f,
                NPC.scale,
                NPC.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0
                );

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			if (drawAura && auraTexture != null)
			{
				Vector2 drawPosition = NPC.Center - Main.screenPosition;
				int auraFrame = auraSingleFrame;
				if (auraFrame <= -1)
				{
					auraFrame = (int)(StateTimer / auraTicksPerFrame) % auraTotalFrames;
				}
                Main.EntitySpriteDraw(auraTexture.Value, drawPosition,
                    auraTexture.Frame(1, auraTotalFrames, 0, auraFrame), auraColor, NPC.rotation,
                    auraTexture.Frame(1, auraTotalFrames).Size() / 2f, NPC.scale, NPC.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
	}

    public abstract class BossState
    {
		public PokemonBossBody boss;
		
        public int duration = 0;
        //if frameEnd is earlier than frameStart, the animation will play backwards.
        public int frameStart = 0;
		public int frameEnd = 0;
		public int ticksPerFrame = 15;

		public abstract Vector2 StateAI();
		public abstract BossState FindNextState();

		private void CalcDuration()
		{
			if (duration == 0)
			{
				duration = (Math.Abs(frameEnd - frameStart) + 1) * ticksPerFrame;
			}
		}

		public void Run()
		{
            if (boss.StateTimer == -1) //New state
            {
				CalcDuration();
				boss.NPC.netUpdate = true;
            }
            if (boss.StateTimer < duration - 1) //Running State
			{
				boss.StateTimer++;
			}
			else //End State
			{
				boss.StateTimer = -1;
				ChangeState();
				boss.bossState.Run();
				return;
			}

            boss.NPC.velocity = StateAI();
        }

		public void ClearState()
		{
			FindNextState();
			boss.bossState = boss.neutralState;
		}

		public void ChangeState()
		{
			BossState nextState = FindNextState();
			//needs some way of syncing boss state over net
			
			boss.bossState = nextState;
		}
    }
}
