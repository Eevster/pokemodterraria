using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MachokeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 11;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,9];
		public override int[] jumpStartEnd => [4,7];
		public override int[] fallStartEnd => [9,9];
		public override float catchRate => 90;
		public override int minLevel => 28;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It can freely detach its jaw to swallow large prey whole. It can become too heavy to move, however."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				if (spawnInfo.Player.ZoneForest) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.2f);
			}
			}

			return 0f;
		}
		
	}

	public class MachokeCritterNPCShiny : MachokeCritterNPC{}
}