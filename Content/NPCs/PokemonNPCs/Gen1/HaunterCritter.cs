using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class HaunterCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 34;
		public override int hitboxHeight => 40;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];

        public override int minLevel => 25;
		public override string[] variants => ["Christmas", "Halloween"];
		public override float catchRate => 90;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.TheDungeon, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.Graveyard, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			float chanceMult = 0.2f;

			if (spawnInfo.Player.ZoneDungeon) {
                return GetSpawnChance(spawnInfo, SpawnCondition.DungeonNormal.Chance * chanceMult);
            }
			if (spawnInfo.Player.ZoneGraveyard) {
                return GetSpawnChance(spawnInfo, 0.5f*chanceMult);
            }

			return 0f;
		}
	}

	public class HaunterCritterNPCShiny : HaunterCritterNPC{}
}