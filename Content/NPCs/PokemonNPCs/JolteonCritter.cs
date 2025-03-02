using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class JolteonCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 24;
		
		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

		public override int minLevel => 40;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Granite,
				new FlavorTextBestiaryInfoElement("It controls 10,000-volt power. When the fur covering its body stands on end, the fur is hard and sharp, like needles."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneGranite) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.001f);
			}

			return 0f;
		}
	}

	public class JolteonCritterNPCShiny : JolteonCritterNPC{}
}