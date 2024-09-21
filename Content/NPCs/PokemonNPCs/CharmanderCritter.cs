using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CharmanderCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 32;

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [14,19];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("The fire on the tip of its tail is a measure of its life. If the Pokémon is healthy, its tail burns intensely."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f;
            }

			return 0f;
		}
	}

	public class CharmanderCritterNPCShiny : CharmanderCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return SpawnCondition.OverworldDay.Chance * 0.5f * 0.00025f;
            }

			return 0f;
		}
	}
}