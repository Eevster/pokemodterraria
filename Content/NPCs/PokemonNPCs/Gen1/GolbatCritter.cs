using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GolbatCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 46;

		public override int moveStyle => 1;

		public override int totalFrames => 4;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [0,2];
		
		public override int[] idleFlyStartEnd => [0,2];
		public override int[] walkFlyStartEnd => [0,2];
		public override int[] attackFlyStartEnd => [3,3];
		public override float catchRate => 90;
		public override int minLevel => 22;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneJungle) {
					return GetSpawnChance(spawnInfo, SpawnCondition.OverworldNight.Chance * 0.5f);
			}
			}

			return 0f;
		}
		
	}

	public class GolbatCritterNPCShiny : GolbatCritterNPC{}
}