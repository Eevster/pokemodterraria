
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class LitwickCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 22;

		public override int moveStyle => 0;

		public override int totalFrames => 20;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [10,14];
		public override int[] walkStartEnd => [0,4];
		public override int[] jumpStartEnd => [15,18];
		public override int[] fallStartEnd => [18,19];
		public override int[] attackStartEnd => [5,9];
        public override float catchRate => 190;
		public override string[] variants => ["Halloween"];

        public override int minLevel => 25;
		
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
			if (spawnInfo.Player.ZoneDungeon) {
                return GetSpawnChance(spawnInfo, SpawnCondition.DungeonNormal.Chance * 0.1f);
            }
			if (spawnInfo.Player.ZoneGraveyard) {
                return GetSpawnChance(spawnInfo, 0.05f);
            }

			return 0f;
		}
	}

	public class LitwickCritterNPCShiny : LitwickCritterNPC{}
}