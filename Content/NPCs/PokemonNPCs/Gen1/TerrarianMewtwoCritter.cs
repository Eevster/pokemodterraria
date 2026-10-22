using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TerrarianMewtwoCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 62;
        public override int hitboxHeight => 66;

        public override int totalFrames => 46;
        public override int animationSpeed => 5;

        public override int moveStyle => 2;
        public override int[] idleStartEnd => [0, 10];
        public override int[] walkStartEnd => [22, 27];
        public override int[] jumpStartEnd => [22, 27];
        public override int[] fallStartEnd => [22, 27];
        public override int[] attackStartEnd => [11, 21];

        public override int[] idleFlyStartEnd => [22, 27];
        public override int[] walkFlyStartEnd => [29, 34];
        public override int[] attackFlyStartEnd => [34, 45];
        public override float catchRate => 3;

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

	public class TerrarianMewtwoCritterNPCShiny : TerrarianMewtwoCritterNPC { }
}