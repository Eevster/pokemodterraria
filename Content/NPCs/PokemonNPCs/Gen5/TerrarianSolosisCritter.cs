
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianSolosisCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 34;
		public override int hitboxHeight => 32;

		

		public override int totalFrames => 25;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,14];
		public override int[] jumpStartEnd => [20,24];
		public override int[] fallStartEnd => [22,23];
		public override int[] attackStartEnd => [13,21];
        public override float catchRate => 200;
		
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

	public class TerrarianSolosisCritterNPCShiny : TerrarianSolosisCritterNPC { }
}