using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class IvysaurCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 26;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,14];

		public override int minLevel => 16;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("When the bud on its back starts swelling, a sweet aroma wafts to indicate the flower's coming bloom."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return SpawnCondition.OverworldDay.Chance * 0.5f;
			}

			return 0f;
		}
	}

	public class IvysaurCritterNPCShiny : IvysaurCritterNPC{
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return SpawnCondition.OverworldDay.Chance * 0.5f * 0.00025f;
			}
			
			return 0f;
		}
	}
}