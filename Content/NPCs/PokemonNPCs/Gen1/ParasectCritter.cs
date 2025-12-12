using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ParasectCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 34;
		public override int hitboxHeight => 44;

		public override int totalFrames => 14;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [8,13];

		public override int minLevel => 24;
		public override float catchRate => 75;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Underground, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
            base.SetBestiary(database, bestiaryEntry);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneNormalUnderground) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.2f);
			}

			return 0f;
		}
		
	}

	public class ParasectCritterNPCShiny : ParasectCritterNPC{}
}