using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VaporeonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 26;

		public override int totalFrames => 19;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,17];
		public override int[] attackSwimStartEnd => [18,18];
        public override float catchRate => 120;
        public override int minLevel => 40;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("It has evolved to be suited for an aquatic life. It can become invisible when it melts away into water."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.001f);
			}

			return 0f;
		}
	}

	public class VaporeonCritterNPCShiny : VaporeonCritterNPC{}
}