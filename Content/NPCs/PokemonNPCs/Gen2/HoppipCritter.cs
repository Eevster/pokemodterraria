using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;
using System;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
    public class HoppipCritterNPC : PokemonWildNPC
    {
        public override int hitboxWidth => 20;
        public override int hitboxHeight => 20;

        public override int totalFrames => 61;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [32, 49];
        public override int[] walkStartEnd => [50, 60];
        public override int[] jumpStartEnd => [35, 39];
        public override int[] fallStartEnd => [8, 31];
        public override int[] attackStartEnd => [0, 7];

        public override int[] idleFlyStartEnd => [8,31];
		public override int[] walkFlyStartEnd => [8,31];
		public override int[] attackFlyStartEnd => [0, 7];
        public override float catchRate => 180;

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
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 1.0f);
            }

            return 0f;
        }
    }

    public class HoppipCritterNPCShiny : HoppipCritterNPC { }
}