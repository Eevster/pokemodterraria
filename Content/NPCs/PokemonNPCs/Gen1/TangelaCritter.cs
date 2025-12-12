using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TangelaCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 9];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [7, 7];
		public override float catchRate => 190;

		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All],
            [(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest || spawnInfo.Player.ZoneJungle)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
			}

			return 0f;
		}
		
	}

	public class TangelaCritterNPCShiny : TangelaCritterNPC{}
}