using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MewtwoCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 44;
        public override int hitboxHeight => 62;

        public override int totalFrames => 39;
        public override int animationSpeed => 7;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [9, 16];
        public override int[] walkStartEnd => [33, 38];
        public override int[] jumpStartEnd => [27, 32];
        public override int[] fallStartEnd => [27, 32];
        public override int[] attackStartEnd => [0, 8];

        public override int[] idleFlyStartEnd => [27, 32];
        public override int[] walkFlyStartEnd => [33, 38];
        public override int[] attackFlyStartEnd => [17, 26];
        public override float catchRate => 3;

		public override float maleChance => -1f;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.00001f);
			}
			}

			return 0f;
		}
		
	}

	public class MewtwoCritterNPCShiny : MewtwoCritterNPC{}
}