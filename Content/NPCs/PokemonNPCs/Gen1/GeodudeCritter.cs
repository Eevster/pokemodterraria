using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GeodudeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 42;
		public override int hitboxHeight => 38;

		public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [0, 3];
		public override int[] jumpStartEnd => [3, 3];
		public override int[] fallStartEnd => [7, 7];
		public override int[] attackStartEnd => [4, 7];
		public override float catchRate => 255;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It can freely detach its jaw to swallow large prey whole. It can become too heavy to move, however."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneNormalUnderground) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.5f);
			}
			}

			return 0f;
		}
		
	}

	public class GeodudeCritterNPCShiny : GeodudeCritterNPC{}
}