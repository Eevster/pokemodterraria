using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WartortleCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 36;

		public override int totalFrames => 17;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,16];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,7];
		public override int[] walkSwimStartEnd => [8,15];
		public override int[] attackSwimStartEnd => [16,16];

		public override int minLevel => 16;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("It is said to live 10,000 years. Its furry tail is popular as a symbol of longevity."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class WartortleCritterNPCShiny : WartortleCritterNPC{}
}