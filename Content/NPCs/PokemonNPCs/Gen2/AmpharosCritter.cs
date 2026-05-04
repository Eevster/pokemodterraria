using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class AmpharosCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;

		public override int totalFrames => 30;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,10];
		public override int[] walkStartEnd => [24,29];
		public override int[] jumpStartEnd => [11,14];
		public override int[] fallStartEnd => [9,10];
        public override int[] attackStartEnd => [15, 23];
        public override float catchRate => 45;
        public override int minLevel => 30;

        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
		];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
			}

			return 0f;
		}
		
	}

	public class AmpharosCritterNPCShiny : AmpharosCritterNPC { }
}
