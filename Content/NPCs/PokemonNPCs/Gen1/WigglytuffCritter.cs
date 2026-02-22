using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WigglytuffCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 46;

		public override int totalFrames => 22;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,13];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [16,16];
        public override int[] attackStartEnd => [14, 21];
        public override float catchRate => 50;
		public override int minLevel => 40;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}

			return 0f;
		}
		
	}

	public class WigglytuffCritterNPCShiny : WigglytuffCritterNPC{}
}