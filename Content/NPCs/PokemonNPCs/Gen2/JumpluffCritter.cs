using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;
using Terraria;
using System;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
    public class JumpluffCritterNPC : PokemonWildNPC
    {
        public override int hitboxWidth => 64;
        public override int hitboxHeight => 60;

        public override int totalFrames => 59;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [20, 29];
        public override int[] walkStartEnd => [41, 58];
        public override int[] jumpStartEnd => [29, 34];
        public override int[] fallStartEnd => [10, 19];
        public override int[] attackStartEnd => [34, 41];

        public override int[] idleFlyStartEnd => [10, 19];
        public override int[] walkFlyStartEnd => [10, 19];
        public override int[] attackFlyStartEnd => [0, 10];

        public override float catchRate => 180;

        public override int minLevel => 27;

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
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.1f);
            }

            return 0f;
        }
    }

    public class JumpluffCritterNPCShiny : JumpluffCritterNPC { }
}