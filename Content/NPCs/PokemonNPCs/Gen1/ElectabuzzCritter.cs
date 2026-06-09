using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ElectabuzzCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 56;
		public override int hitboxHeight => 56;

		public override int totalFrames => 24;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [5,12];
		public override int[] jumpStartEnd => [0,5];
		public override int[] fallStartEnd => [0,5];
        public override int[] attackStartEnd => [12, 23];
        public override float catchRate => 75;

        public override int minLevel => 30;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) 
		{ 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}
			}

			return 0f;
		}
		
	}

	public class ElectabuzzCritterNPCShiny : ElectabuzzCritterNPC{}
}