using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria.Localization;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TinkatinkCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 24;

		public override int totalFrames => 18;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [8,10];
		public override int[] jumpStartEnd => [10,12];
		public override int[] fallStartEnd => [11,11];
        public override int[] attackStartEnd => [12, 17];

        public override float catchRate => 150;

		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble);
			base.SetBestiary(database, bestiaryEntry);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneMarble)
			{
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.5f);
			}

			return 0f;
		}
		
	}

	public class TinkatinkCritterNPCShiny : TinkatinkCritterNPC{}
}