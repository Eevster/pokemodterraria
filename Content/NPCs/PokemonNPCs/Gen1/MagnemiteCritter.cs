using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MagnemiteCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 28;
		public override float moveSpeed => 0.75f;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [0,7];

		public override int[] idleFlyStartEnd => [0,7];
		public override int[] walkFlyStartEnd => [0,7];
		public override int[] attackFlyStartEnd => [8,15];
		public override string[] variants => ["Christmas"];
		public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("The units at its sides generate electromagnetic waves that keep it airborne. It feeds on electricity."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class MagnemiteCritterNPCShiny : MagnemiteCritterNPC{}
}