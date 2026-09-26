using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ShroomishCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 28;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [7,18];
		public override int[] walkStartEnd => [22,27];
		public override int[] jumpStartEnd => [19,20];
		public override int[] fallStartEnd => [20,21];
        public override int[] attackStartEnd => [0, 7];
		public override float catchRate => 255;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, SpawnCondition.SurfaceJungle.Chance * 0.4f);
			}

			return 0f;
		}
	}

	public class ShroomishCritterNPCShiny : ShroomishCritterNPC{}
}
