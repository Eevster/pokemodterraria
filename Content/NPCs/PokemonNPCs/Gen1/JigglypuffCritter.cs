using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class JigglypuffCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;

		public override int totalFrames => 17;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,12];
		public override int[] jumpStartEnd => [13,13];
		public override int[] fallStartEnd => [7,7];
        public override int[] attackStartEnd => [13, 16];
        public override float catchRate => 170;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.5f);
			}

			return 0f;
		}
		
	}

	public class JigglypuffCritterNPCShiny : JigglypuffCritterNPC{}
}