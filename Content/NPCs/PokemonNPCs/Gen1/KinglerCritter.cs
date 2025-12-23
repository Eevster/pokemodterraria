using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KinglerCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 72;
		public override int hitboxHeight => 46;

		public override int totalFrames => 9;
		public override int animationSpeed => 15;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [4,4];
		public override int[] attackStartEnd => [8, 8];
		public override float catchRate => 60;
        public override int minLevel => 28;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneBeach) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.3f);
			}
			}

			return 0f;
		}
		
	}

	public class KinglerCritterNPCShiny : KinglerCritterNPC{}
}