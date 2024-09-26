using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CaterpieCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 20;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [14,14];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It releases a stench from its red antennae to repel enemies. It grows by molting repeatedly."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.Overworld.Chance * 0.5f;
			}

			return 0f;
		}
	}

	public class CaterpieCritterNPCShiny : CaterpieCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return SpawnCondition.Overworld.Chance * 0.5f * 0.00025f;
			}
			
			return 0f;
		}
	}
}