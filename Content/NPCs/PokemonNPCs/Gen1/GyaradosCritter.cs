using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GyaradosCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 88;
		public override int hitboxHeight => 80;

		public override int totalFrames => 15;
		public override int animationSpeed => 8;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 9];
		public override int[] jumpStartEnd => [4, 9];
		public override int[] fallStartEnd => [0, 3];
		public override int[] attackStartEnd => [10, 14];

		public override int[] idleFlyStartEnd => [0, 3];
		public override int[] walkFlyStartEnd => [4, 9];
		public override int[] attackFlyStartEnd => [10, 14];

        public override float catchRate => 50;
        public override int minLevel => 20;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneBeach)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}

			return 0f;
		}
	}

	public class GyaradosCritterNPCShiny : GyaradosCritterNPC{}
}