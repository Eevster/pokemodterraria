using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MimeJrCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

        public override int totalFrames => 10;
        public override int animationSpeed => 10;
        public override int[] idleStartEnd => [0, 2];
        public override int[] walkStartEnd => [3, 7];
        public override int[] jumpStartEnd => [4, 4];
        public override int[] fallStartEnd => [2, 2];
        public override int[] attackStartEnd => [8, 9];

        public override float catchRate => 145;
        
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.3f);
			}

			return 0f;
		}
		
	}

	public class MimeJrCritterNPCShiny : MimeJrCritterNPC{}
}