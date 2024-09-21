using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class RaichuCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 30;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override int minLevel => 25;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("When it is angered, it immediately discharges the energy stored in the pouches in its cheeks."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.001f;
			}

			return 0f;
		}
	}

	public class RaichuCritterNPCShiny : RaichuCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.001f * 0.00025f;
			}
			
			return 0f;
		}
	}
}