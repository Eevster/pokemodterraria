using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SudowoodoCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 48;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 15];
		public override int[] walkStartEnd => [16, 21];
		public override int[] jumpStartEnd => [16, 16];
		public override int[] fallStartEnd => [17, 17];

        public override float catchRate => 65;
        public override int minLevel => 5;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.Clear],
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.Clear]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
			else if (spawnInfo.Player.ZoneForest)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}

			return 0f;
		}
	}

	public class SudowoodoCritterNPCShiny : SudowoodoCritterNPC{}
}