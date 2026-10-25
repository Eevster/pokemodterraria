
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ChandelureCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 90;
		public override int hitboxHeight => 44;

		public override int moveStyle => 1;

		public override int totalFrames => 21;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [16,20];
        public override int[] idleFlyStartEnd => [0, 6];
        public override int[] walkFlyStartEnd => [16, 20];
        public override int[] attackFlyStartEnd => [7, 15];
        public override float catchRate => 190;
        public override string[] variants => ["Halloween"];
        public override int minLevel => 50;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.TheDungeon, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDungeon) {
                return GetSpawnChance(spawnInfo, SpawnCondition.DungeonNormal.Chance * 0.001f);
            }

			return 0f;
		}
	}

	public class ChandelureCritterNPCShiny : ChandelureCritterNPC{}
}