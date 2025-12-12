using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ExeggutorCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 10;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [3,8];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [6,6];
        public override float catchRate => 45;
		public override int minLevel => 30;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.All],
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
            if (spawnInfo.Player.ZoneBeach)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
            }

            return 0f;
		}
		
	}

	public class ExeggutorCritterNPCShiny : ExeggutorCritterNPC{}
}