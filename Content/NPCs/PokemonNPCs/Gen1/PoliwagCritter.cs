using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class PoliwagCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 20;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [2,2];
		public override int[] attackStartEnd => [8,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [12,15];
		public override int[] walkSwimStartEnd => [12,15];
		public override int[] attackSwimStartEnd => [8,11];
		public override float catchRate => 255;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Its skin is so thin, its internal organs are visible. It has trouble walking on its newly grown feet."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return GetSpawnChance(spawnInfo, SpawnCondition.WaterCritter.Chance * 0.5f);
		}
		
	}

	public class PoliwagCritterNPCShiny : PoliwagCritterNPC{}
}