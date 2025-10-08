using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BellsproutCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 26;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8,11];
		public override float catchRate => 255;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Jungle, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.UndergroundJungle, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("It prefers hot and humid environments. It is quick at capturing prey with its vines."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Overworld.Chance+SpawnCondition.Underground.Chance) * 0.8f);
			}

			return 0f;
		}
		
	}

	public class BellsproutCritterNPCShiny : BellsproutCritterNPC{}
}