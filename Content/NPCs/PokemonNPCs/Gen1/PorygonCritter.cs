using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PorygonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 8;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [3,6];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [2, 2];
		public override int[] attackStartEnd => [7,7];
		public override float catchRate => 50;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("The world's first artificially created Pokémon. It can travel through electronic space."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
			}
			}

			return 0f;
		}
		
	}

	public class PorygonCritterNPCShiny : PorygonCritterNPC{}
}