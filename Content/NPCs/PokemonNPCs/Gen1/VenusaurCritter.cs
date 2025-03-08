using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VenusaurCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 50;
		public override int hitboxHeight => 40;

		public override int totalFrames => 25;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,14];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [15,24];

		public override int minLevel => 32;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("After a rainy day, the flower on its back smells stronger. The scent attracts other Pokémon."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class VenusaurCritterNPCShiny : VenusaurCritterNPC{}
}