using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ChanseyCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 22;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [14,14];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,21];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It is said to deliver happiness. Being compassionate, it shares its eggs with injured people."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.05f);
			}

			return 0f;
		}
	}

	public class ChanseyCritterNPCShiny : ChanseyCritterNPC{}
}