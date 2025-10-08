using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PoliwhirlCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [12, 15];
		public override int[] walkSwimStartEnd => [12,15];
		public override int[] attackSwimStartEnd => [8,11];
		public override float catchRate => 120;
        public override int minLevel => 25;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("The spiral pattern on its belly subtly undulates. Staring at it gradually causes drowsiness."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
			}

			return 0f;
		}
		
	}

	public class PoliwhirlCritterNPCShiny : PoliwhirlCritterNPC{}
}