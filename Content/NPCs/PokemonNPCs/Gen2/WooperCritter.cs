using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WooperCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 56;
		public override int hitboxHeight => 40;

		public override int totalFrames => 32;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,11];
		public override int[] walkStartEnd => [24,31];
		public override int[] jumpStartEnd => [13,18];
		public override int[] fallStartEnd => [16,16];
        public override int[] attackStartEnd => [17, 23];
        public override float catchRate => 180;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.UndergroundMushroom.Chance * 0.2f);
			}

			return 0f;
		}

		
	}

	public class WooperCritterNPCShiny : QuagsireCritterNPC{}
}