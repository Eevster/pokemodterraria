using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ExeggutorCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 62;

		public override int totalFrames => 18;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 5];
		public override int[] walkStartEnd => [6, 16];
		public override int[] jumpStartEnd => [9, 9];
		public override int[] fallStartEnd => [13, 13];
		public override int[] attackStartEnd => [17, 17];
		
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