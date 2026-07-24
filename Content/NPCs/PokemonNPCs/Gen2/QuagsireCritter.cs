using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class QuagsireCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 56;
        public override int hitboxHeight => 40;

        public override int totalFrames => 29;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 9];
        public override int[] walkStartEnd => [20, 28];
        public override int[] jumpStartEnd => [10, 13];
        public override int[] fallStartEnd => [13, 13];
        public override int[] attackStartEnd => [14, 19];
        public override float catchRate => 180;

		public override int minLevel => 20;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.UndergroundMushroom, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.01f);
			}
			if (spawnInfo.Player.ZoneGlowshroom) {
				return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance * 0.01f);
			}

			return 0f;
		}
	}

	public class QuagsireCritterNPCShiny : QuagsireCritterNPC{}
}