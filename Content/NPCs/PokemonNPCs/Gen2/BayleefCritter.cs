
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class BayleefCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 40;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,11];
        public override string[] variants => ["Halloween"];

        public override int minLevel => 16;
		
		public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.Jungle, (int)DayTimeStatus.Day, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("The buds that ring its neck give off a spicy aroma that perks people up."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.5f);
            }

			return 0f;
		}
	}

	public class BayleefCritterNPCShiny : BayleefCritterNPC{}
}