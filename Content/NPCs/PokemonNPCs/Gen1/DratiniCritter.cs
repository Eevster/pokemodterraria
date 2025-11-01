using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class DratiniCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8, 11];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,3];
		public override int[] walkSwimStartEnd => [4,7];
		public override int[] attackSwimStartEnd => [8, 11];

		public override float catchRate => 150;
		public override int minLevel => 15;
		
		public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Beach, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("It is called the Mirage Pokémon because so few have seen it, but its shed skin has been found."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneBeach) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.01f);
			}

			return 0f;
		}
		
	}

	public class DratiniCritterNPCShiny : DratiniCritterNPC{}
}