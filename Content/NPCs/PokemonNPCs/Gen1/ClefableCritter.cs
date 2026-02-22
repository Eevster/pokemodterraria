using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ClefableCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 30;
		public override int hitboxHeight => 46;

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,8];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [5,5];
        public override int[] attackStartEnd => [9, 13];
        public override float catchRate => 25;
		public override int minLevel => 40;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.2f);
			}

			return 0f;
		}

		
	}

	public class ClefableCritterNPCShiny : ClefableCritterNPC{}
}