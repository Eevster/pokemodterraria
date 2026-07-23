using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class SlowpokeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
        public override int hitboxHeight => 28;

		public override int totalFrames => 65;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0, 39];
		public override int[] walkStartEnd => [55, 64];
		public override int[] jumpStartEnd => [39, 48];
		public override int[] fallStartEnd => [39, 48];
        public override int[] attackStartEnd => [47, 55];
        public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneBeach) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
			}

			return 0f;
		}
		
	}

	public class SlowpokeCritterNPCShiny : SlowpokeCritterNPC{}
}