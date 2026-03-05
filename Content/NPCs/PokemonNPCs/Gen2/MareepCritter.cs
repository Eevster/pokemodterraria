using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria.Localization;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MareepCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 75;
		public override int hitboxHeight => 60;

		public override int totalFrames => 22;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [6,15];
		public override int[] jumpStartEnd => [6,15];
		public override int[] fallStartEnd => [0,6];
        public override int[] attackStartEnd => [16, 21];

        public override float catchRate => 200;

		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
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
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
		
	}

	public class MareepCritterNPCShiny : MareepCritterNPC{}
}