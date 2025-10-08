using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class NidoranMCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 24;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,9];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [10,15];
		public override float catchRate => 235;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It scans its surroundings by raising its ears out of the grass. Its toxic horn is for protection."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class NidoranMCritterNPCShiny : NidoranMCritterNPC{}
}