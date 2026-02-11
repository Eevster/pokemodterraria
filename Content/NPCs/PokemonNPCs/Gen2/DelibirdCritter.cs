using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DelibirdCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 57;

		public override int animationSpeed => 5;

		public override int[] idleStartEnd => [0, 16];

		public override int[] walkStartEnd => [17, 26];

		public override int[] jumpStartEnd => [27, 32];

		public override int[] fallStartEnd => [33, 40];

		public override int[] attackStartEnd => [41, 50];

		public override float catchRate => 45;
        public override int minLevel => 20;

        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneSnow)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}

			return 0f;
		}
		
	}

	public class DelibirdCritterNPCShiny : DelibirdCritterNPC{}
}