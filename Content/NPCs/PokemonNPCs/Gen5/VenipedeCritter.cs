
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VenipedeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 60;
		public override int hitboxHeight => 44;

		

		public override int totalFrames => 36;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [7,15];
		public override int[] walkStartEnd => [16,22];
		public override int[] jumpStartEnd => [0,6];
		public override int[] fallStartEnd => [2,4];
		public override int[] attackStartEnd => [0,6];
        public override float catchRate => 255;
		
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
                return GetSpawnChance(spawnInfo, 0.8f);
            }

			return 0f;
		}
	}

	public class VenipedeCritterNPCShiny : VenipedeCritterNPC{}
}