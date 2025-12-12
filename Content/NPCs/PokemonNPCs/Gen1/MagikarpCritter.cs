using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class MagikarpCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 28;

		public override int totalFrames => 8;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [1, 3];
		public override int[] walkStartEnd => [0, 4];
		public override int[] jumpStartEnd => [3, 4];
		public override int[] fallStartEnd => [5, 7];
		public override int[] attackStartEnd => [3, 7];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [6, 6];
		public override int[] walkSwimStartEnd => [5, 7];
		public override int[] attackSwimStartEnd => [3, 7];
		public override float catchRate => 255;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return GetSpawnChance(spawnInfo, SpawnCondition.WaterCritter.Chance * 0.5f);
		}
	}

	public class MagikarpCritterNPCShiny : MagikarpCritterNPC{}
}