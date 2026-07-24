
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianDuosionCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 35;
		public override int hitboxHeight => 59;

		

		public override int totalFrames => 22;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [13,18];
		public override int[] jumpStartEnd => [18,21];
		public override int[] fallStartEnd => [21,21];
		public override int[] attackStartEnd => [6,13];
        public override float catchRate => 100;

        public override int minLevel => 32;

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
                return GetSpawnChance(spawnInfo, 0.5f);
            }

			return 0f;
		}
	}

	public class TerrarianDuosionCritterNPCShiny : TerrarianDuosionCritterNPC { }
}