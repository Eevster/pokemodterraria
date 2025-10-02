using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GolduckCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 12;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 10];
		public override int[] jumpStartEnd => [5, 5];
		public override int[] fallStartEnd => [9, 9];
		public override float catchRate => 75;
		public override int minLevel => 33;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Old tales tell of Golduck punishing those that defiled its river. The guilty were dragged into the water and taken away."));
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

	public class GolduckCritterNPCShiny : GolduckCritterNPC{}
}