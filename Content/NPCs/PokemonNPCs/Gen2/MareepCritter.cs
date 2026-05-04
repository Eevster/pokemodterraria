using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria.Localization;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MareepCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 24;

		public override int totalFrames => 26;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [21,25];
		public override int[] jumpStartEnd => [7,10];
		public override int[] fallStartEnd => [0,2];
        public override int[] attackStartEnd => [11, 20];

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
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.7f);
			}

			return 0f;
		}
		
	}

	public class MareepCritterNPCShiny : MareepCritterNPC{}
}