using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PaldeanWooperCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 30;
        public override int hitboxHeight => 30;

        public override int totalFrames => 40;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [13, 17];
        public override int[] walkStartEnd => [0, 6];
        public override int[] jumpStartEnd => [18, 22];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [7, 14];
        public override bool canSwim => true;
        public override int[] idleSwimStartEnd => [30, 35];
        public override int[] walkSwimStartEnd => [36, 39];
        public override int[] attackSwimStartEnd => [24, 29];

        public override float catchRate => 255;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneJungle) {
                return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
            	}

			return 0f;
		}

		
	}

	public class PaldeanWooperCritterNPCShiny : PaldeanWooperCritterNPC { }
}