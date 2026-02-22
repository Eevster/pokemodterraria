using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SandshrewCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 36;

		public override int totalFrames => 20;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,14];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [8,8];
        public override int[] attackStartEnd => [15, 19];
        public override float catchRate => 255;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneDesert) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            	}

			return 0f;
		}

		
	}

	public class SandshrewCritterNPCShiny : SandshrewCritterNPC{}
}