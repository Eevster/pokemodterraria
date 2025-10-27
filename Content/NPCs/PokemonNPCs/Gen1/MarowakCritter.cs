using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MarowakCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 10;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,8];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [8,8];

		public override float catchRate => 75;
		public override int minLevel => 28;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Desert, (int)DayTimeStatus.Night, (int)WeatherStatus.All],
            [(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("This Pokémon overcame its sorrow to evolve a sturdy new body. Marowak faces its opponents bravely, using a bone as a weapon."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if (spawnInfo.Player.ZoneDesert)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.2f);
            }
            if (spawnInfo.Player.ZoneNormalUnderground)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.1f);
            }

            return 0f;
		}
		
	}

	public class MarowakCritterNPCShiny : MarowakCritterNPC{}
}