
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DuosionCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
        public override int hitboxHeight => 34;
		
		public override int totalFrames => 20;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [16,19];
		public override int[] walkStartEnd => [8,11];
		public override int[] jumpStartEnd => [12,15];
		public override int[] fallStartEnd => [15,16];
		public override int[] attackStartEnd => [0,7];
        public override float catchRate => 100;

        public override int minLevel => 32;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.UndergroundJungle, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle);
            base.SetBestiary(database, bestiaryEntry);
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGlowshroom) {
                return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance*0.4f);
            }

			return 0f;
		}
	}

	public class DuosionCritterNPCShiny : DuosionCritterNPC { }
}