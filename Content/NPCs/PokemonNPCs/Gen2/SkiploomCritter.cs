using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;
using System;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
    public class SkiploomCritterNPC : PokemonWildNPC
    {
        public override int hitboxWidth => 30;
        public override int hitboxHeight => 26;

        public override int totalFrames => 44;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [22, 35];
        public override int[] walkStartEnd => [36, 43];
        public override int[] jumpStartEnd => [8, 11];
        public override int[] fallStartEnd => [12, 13];
        public override int[] attackStartEnd => [14, 21];

        public override int[] idleFlyStartEnd => [8,13];
		public override int[] walkFlyStartEnd => [8,13];
		public override int[] attackFlyStartEnd => [0, 7];

        public override float catchRate => 180;

        public override int minLevel => 18;

        public override int[][] spawnConditions =>
		[
            [(int)SpawnArea.Surface, (int)DayTimeStatus.Day, (int)WeatherStatus.Windy]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            base.SetBestiary(database, bestiaryEntry);
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest && Math.Abs(Main.windSpeedCurrent) >= 0.4f)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.9f);
            }

            return 0f;
        }
    }

    public class SkiploomCritterNPCShiny : SkiploomCritterNPC { }
}