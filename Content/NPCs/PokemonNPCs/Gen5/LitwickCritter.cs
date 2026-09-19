
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
		public override int animationSpeed => 11;
		public override int[] idleStartEnd => [13,15];
		public override int[] walkStartEnd => [11,12];
		public override int[] jumpStartEnd => [15,19];
		public override int[] fallStartEnd => [18,19];
		public override int[] attackStartEnd => [0,12];
        public override float catchRate => 190;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Granite, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneCorrupt) {
                return GetSpawnChance(spawnInfo, 0.08f);
            }

			return 0f;
		}
	}

	public class LitwickCritterNPCShiny : LitwickCritterNPC{}
}