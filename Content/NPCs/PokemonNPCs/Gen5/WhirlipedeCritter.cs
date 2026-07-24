
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WhirlipedeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 70;
		public override int hitboxHeight => 48;

		

		public override int totalFrames => 22;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [11,21];
		public override int[] walkStartEnd => [6,9];
		public override int[] jumpStartEnd => [4,5];
		public override int[] fallStartEnd => [5,5];
		public override int[] attackStartEnd => [0,3];
        public override float catchRate => 120;

        public override int minLevel => 22;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Granite, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
                return GetSpawnChance(spawnInfo, 0.1f);
            }

			return 0f;
		}
	}

	public class WhirlipedeCritterNPCShiny : WhirlipedeCritterNPC{}
}