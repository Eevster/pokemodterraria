using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SandslashCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 64;
		public override int hitboxHeight => 64;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
		public override float catchRate => 90;
        public override int minLevel => 22;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It digs deep burrows to live in. When in danger, it rolls up its body to withstand attacks."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneDesert) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.1f);
            	}
			}

			return 0f;
		}

		
	}

	public class SandslashCritterNPCShiny : SandslashCritterNPC{}
}