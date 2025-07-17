using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VictreebelCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 50;

		public override int totalFrames => 8;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [3,3];
		public override int[] attackStartEnd => [4,7];

		public override int minLevel => 40;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("In its mouth, it pools a fragrant nectar-like fluid. The fluid is really an acid that dissolves anything."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Overworld.Chance+SpawnCondition.Underground.Chance) * 0.1f);
			}

			return 0f;
		}
		
	}

	public class VictreebelCritterNPCShiny : VictreebelCritterNPC{}
}