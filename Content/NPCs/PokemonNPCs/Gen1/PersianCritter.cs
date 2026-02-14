using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PersianCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 13;
		public override int animationSpeed => 9;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,11];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [12, 12];
		public override float catchRate => 90;
		public override int minLevel => 28;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}
			}

			return 0f;
		}
		
	}

	public class PersianCritterNPCShiny : PersianCritterNPC{}
}