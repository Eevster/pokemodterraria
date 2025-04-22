using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GalvantulaCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 60;
		public override int hitboxHeight => 48;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [8,11];

		public override int minLevel => 50;
		public override float catchRate => 75;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
				new FlavorTextBestiaryInfoElement("It launches electrified fur from its abdomen as its means of attack. Opponents hit by the fur could be in for three full days and nights of paralysis."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGranite) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.004f);
			}

			return 0f;
		}
		
	}

	public class GalvantulaCritterNPCShiny : GalvantulaCritterNPC{}
}