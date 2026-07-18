using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class FurretCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 23;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [6,12];
		public override int[] walkStartEnd => [19,22];
		public override int[] jumpStartEnd => [13,19];
		public override int[] fallStartEnd => [17,18];
		public override int[] attackStartEnd => [0,6];
		public override float catchRate => 120;
		
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
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.09f);
			}

			return 0f;
		}
	}

	public class FurretCritterNPCShiny : FurretCritterNPC{}
}