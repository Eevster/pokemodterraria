using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VoltorbCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

        public override float moveSpeed => 1.5f;

        public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override bool canRotate => true;
		public override float catchRate => 190;

		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Marble, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble);
			base.SetBestiary(database, bestiaryEntry);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneMarble) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance+SpawnCondition.Cavern.Chance) * 0.5f);
			}

			return 0f;
		}
	}

	public class VoltorbCritterNPCShiny : VoltorbCritterNPC{}
}