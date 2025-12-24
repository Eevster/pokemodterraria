using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DragoniteCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 64;

		public override int totalFrames => 24;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [16,19];
		public override int[] walkFlyStartEnd => [16,19];
		public override int[] attackFlyStartEnd => [20,23];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [16,19];
		public override int[] walkSwimStartEnd => [16,19];
		public override int[] attackSwimStartEnd => [20,23];

        public override float catchRate => 20;
        public override int minLevel => 55;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneBeach) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
			}
			}

			return 0f;
		}
		
	}

	public class DragoniteCritterNPCShiny : DragoniteCritterNPC{}
}