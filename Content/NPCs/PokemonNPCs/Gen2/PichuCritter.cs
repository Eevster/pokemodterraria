using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PichuCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [10,12];
		public override int[] fallStartEnd => [13,15];
		public override int[] attackStartEnd => [18,25];
		public override float catchRate => 190;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
			base.SetBestiary(database, bestiaryEntry);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneForest)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.08f);
			}

			return 0f;
		}
	}

	public class PichuCritterNPCShiny : PichuCritterNPC{}
}