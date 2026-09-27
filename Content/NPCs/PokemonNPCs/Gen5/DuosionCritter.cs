
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DuosionCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
        public override int hitboxHeight => 24;

		public override int moveStyle => 1;
		
		public override int totalFrames => 20;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [11,19];
		public override int[] walkStartEnd => [7,10];
		public override int[] idleFlyStartEnd => [11,19];
        public override int[] walkFlyStartEnd => [7,10];
        public override int[] attackFlyStartEnd => [0,6];
        public override float catchRate => 100;

        public override int minLevel => 32;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.UndergroundMushroom, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGlowshroom) {
                return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance*0.4f);
            }

			return 0f;
		}
	}

	public class DuosionCritterNPCShiny : DuosionCritterNPC { }
}