using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MagnetonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [0,7];

		public override int[] idleFlyStartEnd => [0,7];
		public override int[] walkFlyStartEnd => [0,7];
		public override int[] attackFlyStartEnd => [8,15];

        public override int minLevel => 30;

		public override string[] variants => ["Christmas"];
		public override float catchRate => 60;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface); 
			base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class MagnetonCritterNPCShiny : MagnetonCritterNPC{}
}