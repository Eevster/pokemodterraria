
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ScolipedeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 104;
		public override int hitboxHeight => 94;

		

		public override int totalFrames => 30;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [13,23];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [23,29];
		public override int[] fallStartEnd => [28,29];
		public override int[] attackStartEnd => [0,6];
        public override float catchRate => 45;

        public override int minLevel => 30;

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
                return GetSpawnChance(spawnInfo, 0.08f);
            }

			return 0f;
		}
	}

	public class ScolipedeCritterNPCShiny : ScolipedeCritterNPC{}
}