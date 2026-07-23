using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SlowkingCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 56;

        public override int totalFrames => 38;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 13];
        public override int[] walkStartEnd => [29, 37];
        public override int[] jumpStartEnd => [14, 20];
        public override int[] fallStartEnd => [17, 19];
        public override int[] attackStartEnd => [20, 28];
        public override float catchRate => 75;
        public override int minLevel => 36;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneBeach) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
			}
			}

			return 0f;
		}
		
	}

	public class SlowkingCritterNPCShiny : SlowkingCritterNPC{}
}