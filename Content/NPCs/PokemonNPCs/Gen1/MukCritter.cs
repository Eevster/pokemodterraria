using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MukCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 36;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [10,16];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [17,20];
		public override int[] fallStartEnd => [21,21];
		public override int[] attackStartEnd => [4,9];
		public override float catchRate => 75;
        public override int minLevel => 38;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneDungeon) {
					return GetSpawnChance(spawnInfo, SpawnCondition.DungeonNormal.Chance * 0.3f);
			}
			}

			return 0f;
		}
		
	}

	public class MukCritterNPCShiny : MukCritterNPC{}
}