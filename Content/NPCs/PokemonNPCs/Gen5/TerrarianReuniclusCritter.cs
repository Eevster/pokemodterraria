
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianReuniclusCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 94;
		public override int hitboxHeight => 84;

		

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,10];
		public override int[] walkStartEnd => [10,14];
		public override int[] jumpStartEnd => [21,25];
		public override int[] fallStartEnd => [24,24];
		public override int[] attackStartEnd => [15,21];
        public override float catchRate => 50;

        public override int minLevel => 41;

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
                return GetSpawnChance(spawnInfo, 0.09f);
            }

			return 0f;
		}
	}

	public class TerrarianReuniclusCritterNPCShiny : TerrarianReuniclusCritterNPC { }
}