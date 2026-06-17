using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class AlolanDugtrioCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
		public override int[] attackStartEnd => [8,13];

		public override float catchRate => 50;
		public override int minLevel => 25;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.UndergroundBeach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if ((spawnInfo.Player.ZoneDirtLayerHeight || spawnInfo.Player.ZoneRockLayerHeight) && (spawnInfo.Player.Center.X < 340*16 || spawnInfo.Player.Center.X > 16*(Main.maxTilesX - 340))) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Underground.Chance + SpawnCondition.Cavern.Chance) * 0.3f);
			}

			return 0f;
		}
		
	}

	public class AlolanDugtrioCritterNPCShiny : AlolanDugtrioCritterNPC{}
}