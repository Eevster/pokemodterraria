using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;
using Microsoft.Xna.Framework;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KoffingCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 2;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] attackStartEnd => [1,1];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [1,1];

		public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It can freely detach its jaw to swallow large prey whole. It can become too heavy to move, however."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
			}

			return 0f;
		}

		public override void ExtraEffects() {
			if(Main.rand.NextBool(10)){
				int goreIndex = Gore.NewGore(NPC.GetSource_FromThis(), NPC.position, Vector2.Zero, Main.rand.Next(220, 223), 1f);
				Main.gore[goreIndex].scale = 0.5f;
				Main.gore[goreIndex].position = NPC.position + 0.5f*hitboxWidth*Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
				Main.gore[goreIndex].velocity = 0.05f*hitboxWidth*(Main.gore[goreIndex].position-NPC.position).SafeNormalize(Vector2.UnitX);
			}
		}
		
	}

	public class KoffingCritterNPCShiny : KoffingCritterNPC{}
}