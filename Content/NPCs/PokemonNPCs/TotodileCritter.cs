using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TotodileCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;

		public override int totalFrames => 13;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [12,12];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [6,11];
		public override int[] attackSwimStartEnd => [12,12];


		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("It has a habit of biting everything with its well-developed jaws. Even its Trainer needs to be careful."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class TotodileCritterNPCShiny : TotodileCritterNPC{}
}