
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class LampentCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 38;
		public override int hitboxHeight => 38;

		public override int moveStyle => 1;

		public override int totalFrames => 19;
		public override int animationSpeed => 11;
		public override int[] idleStartEnd => [9,18];
		public override int[] walkStartEnd => [6,8];
        public override int[] idleFlyStartEnd => [9, 18];
        public override int[] walkFlyStartEnd => [6, 8];
        public override int[] attackFlyStartEnd => [0, 5];
        public override float catchRate => 190;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Granite, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneCorrupt) {
                return GetSpawnChance(spawnInfo, 0.02f);
            }

			return 0f;
		}
	}

	public class LampentCritterNPCShiny : LampentCritterNPC{}
}