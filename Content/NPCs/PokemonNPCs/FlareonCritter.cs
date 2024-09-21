using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class FlareonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 28;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,21];

		public override int minLevel => 25;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It has a flame sac in its body. Its body temperature tops 1,650 degrees Fahrenheit before battle."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.001f;
			}

			return 0f;
		}
	}

	public class FlareonCritterNPCShiny : FlareonCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.OverworldDay.Chance * 0.001f * 0.00025f;
			}
			
			return 0f;
		}
	}
}