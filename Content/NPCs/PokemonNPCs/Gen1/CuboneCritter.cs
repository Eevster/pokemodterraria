using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CuboneCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 11;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,9];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [7,7];
		public override float catchRate => 190;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Desert, (int)DayTimeStatus.Night, (int)WeatherStatus.All],
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("When the memory of its departed mother brings it to tears, its cries echo mournfully within the skull it wears on its head."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.5f);
			}
            if (spawnInfo.Player.ZoneNormalUnderground)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.2f);
            }

            return 0f;
		}
		
	}

	public class CuboneCritterNPCShiny : CuboneCritterNPC{}
}