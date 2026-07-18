using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SentretCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 32;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [6,19];
		public override int[] walkStartEnd => [25,31];
		public override int[] jumpStartEnd => [19,24];
		public override int[] fallStartEnd => [20,21];
		public override int[] attackStartEnd => [0,5];
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
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.4f);
			}

			return 0f;
		}
	}

	public class SentretCritterNPCShiny : SentretCritterNPC { }
}