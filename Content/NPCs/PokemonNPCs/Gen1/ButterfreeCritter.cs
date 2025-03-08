using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ButterfreeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 36;
		public override float moveSpeed => 0.75f;

		public override int totalFrames => 7;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [0,6];

		public override int[] idleFlyStartEnd => [0,6];
		public override int[] walkFlyStartEnd => [0,6];
		public override int[] attackFlyStartEnd => [0,6];
		public override int minLevel => 30;
		public override string[] variants => ["Christmas"];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It loves the nectar of flowers and can locate flower patches that have even tiny amounts of pollen."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class ButterfreeCritterNPCShiny : ButterfreeCritterNPC{}
}