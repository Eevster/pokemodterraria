using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class LickitungCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 36;

        public override int totalFrames => 24;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [7, 11];
        public override int[] walkStartEnd => [12, 19];
        public override int[] jumpStartEnd => [20, 23];
        public override int[] fallStartEnd => [21, 22];
        public override int[] attackStartEnd => [0, 6];
        public override float catchRate => 190;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
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

	public class LickitungCritterNPCShiny : LickitungCritterNPC{}
}