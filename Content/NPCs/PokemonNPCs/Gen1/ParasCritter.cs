using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ParasCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 24;

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [8,13];
		public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				new FlavorTextBestiaryInfoElement("Mushrooms named tochukaso grow on its back. They grow along with the host Paras."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.8f);
			}

			return 0f;
		}
		
	}

	public class ParasCritterNPCShiny : ParasCritterNPC{}
}