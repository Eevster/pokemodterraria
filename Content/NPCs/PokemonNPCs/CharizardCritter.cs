using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CharizardCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;

		public override int totalFrames => 42;
		public override int animationSpeed => 7;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [15,15];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [22,29];

		public override int[] idleFlyStartEnd => [16,21];
		public override int[] walkFlyStartEnd => [36,41];
		public override int[] attackFlyStartEnd => [30,35];

		public override int minLevel => 36;
		public override string[] variants => ["Christmas"];


		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
				new FlavorTextBestiaryInfoElement("It is said that Charizard's fire burns hotter if it has experienced harsh battles."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneDesert) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class CharizardCritterNPCShiny : CharizardCritterNPC{}
}