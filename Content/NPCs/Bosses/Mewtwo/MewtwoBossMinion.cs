using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace Pokemod.Content.NPCs.Bosses.Mewtwo
{
	// The minions spawned when the body spawns
	// Please read MinionBossBody.cs first for important comments, they won't be explained here again
	public class MewtwoBossMinion : ModNPC
	{
        Asset<Texture2D> TrailTexture = null;
        // This is a neat trick that uses the fact that NPCs have all NPC.ai[] values set to 0f on spawn (if not otherwise changed).
        // We set ParentIndex to a number in the body after spawning it. If we set ParentIndex to 3, NPC.ai[0] will be 4. If NPC.ai[0] is 0, ParentIndex will be -1.
        // Now combine both facts, and the conclusion is that if this NPC spawns by other means (not from the body), ParentIndex will be -1, allowing us to distinguish
        // between a proper spawn and an invalid/"cheated" spawn
        public int ParentIndex {
			get => (int)NPC.ai[0] - 1;
			set => NPC.ai[0] = value + 1;
		}

		public Player targetPlayer;

		public bool HasParent => ParentIndex > -1;

		public float PositionOffset {
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}

		public const float RotationTimerMax = 360;
		public ref float RotationTimer => ref NPC.ai[2];

		public int shootTimerMax = 150;
        public int shootTimer = 0;


		// Helper method to determine the body type
		public static int BodyType() {
			return ModContent.NPCType<MewtwoBossBody>();
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 1;

			// By default enemies gain health and attack if hardmode is reached. this NPC should not be affected by that
			NPCID.Sets.DontDoHardmodeScaling[Type] = true;
			// Enemies can pick up coins and be respawned automatically, let's prevent it for this NPC since we don't want this enemy to respawn outside of a boss fight.
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			// Specify the debuffs it is immune to. Most NPCs are immune to Confused.
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			// Optional: If you don't want this NPC to show on the bestiary (if there is no reason to show a boss minion separately)
			// Make sure to remove SetBestiary code as well
			// NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers() {
			//	Hide = true // Hides this NPC from the bestiary
			// };
			// NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);
		}

		public override void SetDefaults() {
			NPC.width = 30;
			NPC.height = 30;
			NPC.damage = 30;
			NPC.defense = 0;
			NPC.lifeMax = 2000;
			NPC.HitSound = SoundID.NPCHit9;
			NPC.DeathSound = SoundID.NPCDeath12;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.knockBackResist = 0.4f;
			NPC.netAlways = true;

			NPC.aiStyle = -1;
			TrailTexture = ModContent.Request<Texture2D>("Pokemod/Content/NPCs/Bosses/Mewtwo/MewtwoBossMinionTrail");
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// Makes it so whenever you beat the boss associated with it, it will also get unlocked immediately
			int associatedNPCType = BodyType();
			bestiaryEntry.UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[associatedNPCType], quickUnlock: true);

			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
			});
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void OnKill() {
			// Boss minions typically have a chance to drop an additional heart item in addition to the default chance
			Player closestPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];

			if (Main.rand.NextBool(2) && closestPlayer.statLife < closestPlayer.statLifeMax2) {
				Item.NewItem(NPC.GetSource_Loot(), NPC.getRect(), ItemID.Heart);
			}
		}

		public override void HitEffect(NPC.HitInfo hit) {
			if (NPC.life <= 0) {
				// If this NPC dies, spawn some visuals

				int dustType = 59;

				for (int i = 0; i < 20; i++) {
					Vector2 velocity = NPC.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
					Dust dust = Dust.NewDustPerfect(NPC.Center, dustType, velocity, 26, Color.White, Main.rand.NextFloat(1.5f, 2.4f));

					dust.noLight = true;
					dust.noGravity = true;
					dust.fadeIn = Main.rand.NextFloat(0.3f, 0.8f);
				}
			}
		}

		public override void AI() {
			if (Despawn()) {
				return;
			}

            targetPlayer = FindTarget();

            MoveInFormation();

			LookAtPlayer();

			ShootLaser();
		}

		private bool Despawn() {
			if (Main.netMode != NetmodeID.MultiplayerClient &&
				(!HasParent || !Main.npc[ParentIndex].active || Main.npc[ParentIndex].type != BodyType())) {
				// * Not spawned by the boss body (didn't assign a position and parent) or
				// * Parent isn't active or
				// * Parent isn't the body
				// => invalid, kill itself without dropping any items
				NPC.active = false;
				NPC.life = 0;
				NetMessage.SendData(MessageID.SyncNPC, number: NPC.whoAmI);
				return true;
			}
			return false;
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

        private void MoveInFormation() {
			NPC parentNPC = Main.npc[ParentIndex];

			// This basically turns the NPCs PositionIndex into a number between 0f and TwoPi to determine where around
			// the main body it is positioned at
			float rad = (float)PositionOffset * MathHelper.TwoPi;

			// Add some slight uniform rotation to make the eyes move, giving a chance to touch the player and thus helping melee players
			RotationTimer += 0.5f;
			if (RotationTimer > RotationTimerMax) {
				RotationTimer = 0;
			}

			// Since RotationTimer is in degrees (0..360) we can convert it to radians (0..TwoPi) easily
			float continuousRotation = MathHelper.ToRadians(RotationTimer);
			rad += continuousRotation;
			if (rad > MathHelper.TwoPi) {
				rad -= MathHelper.TwoPi;
			}
			else if (rad < 0) {
				rad += MathHelper.TwoPi;
			}

			float distanceFromBody = parentNPC.width + NPC.width;

			// offset is now a vector that will determine the position of the NPC based on its index
			Vector2 offset = Vector2.One.RotatedBy(rad) * distanceFromBody;

			Vector2 destination = parentNPC.Center + offset;
			Vector2 toDestination = destination - NPC.Center;
			Vector2 toDestinationNormalized = toDestination.SafeNormalize(Vector2.Zero);

			float speed = 14f;
			float inertia = 20;

			Vector2 moveTo = toDestinationNormalized * speed;
			NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;
		}

        private void LookAtPlayer()
		{
			Vector2 toPlayer = targetPlayer.Center - NPC.Center;

            NPC.rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
        }

        private void ShootLaser()
		{
			float distanceToPlayer = (targetPlayer.Center - NPC.Center).Length();
			if (distanceToPlayer > 1500)
			{
				shootTimer = (int)(Main.rand.NextFloat(0.8f, 1.2f) * shootTimerMax);
			}
			else if (shootTimer <= 0)
			{
                shootTimer = (int)(Main.rand.NextFloat(0.8f, 1.2f) * shootTimerMax);
                int projectileSpeed = 10;
                var bullet = NPC.GetSource_FromAI();
                Vector2 position = NPC.position;
                Vector2 direction = targetPlayer.Center - NPC.Center;
                Vector2 velocity = direction.SafeNormalize(Vector2.UnitY) * projectileSpeed;
                int type = ProjectileID.DeathLaser;
                int damage = 30;

                Projectile laser = Main.projectile[Projectile.NewProjectile(bullet, position, velocity, type, damage, 0f, -1)];
            }

			shootTimer --;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC parentNPC = Main.npc[ParentIndex];

            Texture2D texture = TrailTexture.Value;

            Vector2 startPosition = NPC.Center;
			Vector2 targetPosition = parentNPC.Center - Vector2.UnitY * 52;
            Vector2 direction = (targetPosition - NPC.Center).SafeNormalize(-Vector2.UnitY) * texture.Height;
            int trailLength = (int)((startPosition - targetPosition).Length() / texture.Height);

            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            for (int k = 0; k <= trailLength; k++)
            {
                Vector2 drawPos = (startPosition + k * direction - Main.screenPosition) + drawOrigin;
                Color color = drawColor;
                Main.EntitySpriteDraw(texture, drawPos, null, color, direction.ToRotation() + MathHelper.PiOver2, drawOrigin, NPC.scale, SpriteEffects.None, 0);

            }

            return true;
        }  
    }
}
