using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GoldeenCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
		public override float catchRate => 225;

		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
				float chance = 0.4f;
				return GetSpawnChance(spawnInfo, SpawnCondition.Ocean.Chance * chance) + GetSpawnChance(spawnInfo, SpawnCondition.CaveJellyfish.Chance * 0.5f * chance);
			}

			return 0f;
		}
		
	}

	public class GoldeenCritterNPCShiny : GoldeenCritterNPC{}
}