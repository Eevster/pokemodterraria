using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PidgeyCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 26;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [8,11];
		public override int[] attackFlyStartEnd => [12,15];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It is docile and prefers to avoid conflict. If disturbed, however, it can ferociously strike back."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class PidgeyCritterNPCShiny : PidgeyCritterNPC{}
}