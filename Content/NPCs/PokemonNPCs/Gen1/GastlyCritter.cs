using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GastlyCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;
		public override float moveSpeed => 0.75f;

		public override int totalFrames => 9;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [0,2];

		public override int[] idleFlyStartEnd => [0,2];
		public override int[] walkFlyStartEnd => [0,2];
		public override int[] attackFlyStartEnd => [3,8];
		
		public override string[] variants => ["Christmas", "Halloween"];
		public override float catchRate => 190;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.TheCorruption, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.TheCrimson, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				new FlavorTextBestiaryInfoElement("It's a Pokémon born from poison gases. It defeats even the largest foes by enveloping them in gas."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Crimson.Chance+SpawnCondition.Corruption.Chance) * 0.5f);
			}

			return 0f;
		}
	}

	public class GastlyCritterNPCShiny : GastlyCritterNPC{}
}