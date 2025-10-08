using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GengarCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

        public override int moveStyle => 2;

        public override int totalFrames => 18;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [10, 10];
        public override int[] fallStartEnd => [7, 7];

        public override int[] attackStartEnd => [10, 13];

        public override int[] idleFlyStartEnd => [14, 17];
        public override int[] walkFlyStartEnd => [14, 17];
        public override int[] attackFlyStartEnd => [10, 13];

        public override float catchRate => 50;
        public override string[] variants => ["Halloween"];
        public override int minLevel => 40;
        
        public override int[][] spawnConditions =>
		[
			[(int)SpawnArea.TheCorruption, (int)DayTimeStatus.All, (int)WeatherStatus.All],
			[(int)SpawnArea.TheCrimson, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                new FlavorTextBestiaryInfoElement("Lurking in the shadowy corners of rooms, it awaits chances to steal its prey's life force."));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson)
            {
                return GetSpawnChance(spawnInfo, (SpawnCondition.Crimson.Chance + SpawnCondition.Corruption.Chance) * 0.2f);
            }

            return 0f;
        }
		
	}

	public class GengarCritterNPCShiny : GengarCritterNPC{}
}