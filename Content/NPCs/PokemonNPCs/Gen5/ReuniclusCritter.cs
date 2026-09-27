
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ReuniclusCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
        public override int hitboxHeight => 40;

		public override int totalFrames => 15;
		public override int animationSpeed => 6;

        public override int moveStyle => 1;
        public override int[] idleStartEnd => [5, 9];
        public override int[] walkStartEnd => [10, 14];
        public override int[] idleFlyStartEnd => [5, 9];
        public override int[] walkFlyStartEnd => [10, 14];
        public override int[] attackFlyStartEnd => [0, 4];
        public override float catchRate => 50;

        public override int minLevel => 41;

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
                return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance*0.09f);
            }

			return 0f;
		}
	}

	public class ReuniclusCritterNPCShiny : ReuniclusCritterNPC { }
}