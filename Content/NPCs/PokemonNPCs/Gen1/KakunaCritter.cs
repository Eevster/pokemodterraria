using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KakunaCritterNPC : PokemonWildNPC
	{
		public override float moveSpeed => 0.2f;

		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [0,0];

		public override int minLevel => 7;
		public override float catchRate => 120;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("While awaiting evolution, it hides from predators under leaves and in nooks of branches."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.8f);
			}

			return 0f;
		}
	}

	public class KakunaCritterNPCShiny : KakunaCritterNPC{}
}