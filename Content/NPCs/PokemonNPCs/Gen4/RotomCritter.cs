using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class RotomCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 42;

        public override int totalFrames => 19;
        public override int animationSpeed => 4;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 7];
        public override int[] walkStartEnd => [8, 13];
        public override int[] attackStartEnd => [14, 18];

        public override int[] idleFlyStartEnd => [0, 7];
        public override int[] walkFlyStartEnd => [8, 13];
        public override int[] attackFlyStartEnd => [14, 18];

        public override float catchRate => 45;
        public override int minLevel => 16;

        public override float maleChance => -1f;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.TheDungeon, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon);
            base.SetBestiary(database, bestiaryEntry);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDungeon)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.DungeonNormal.Chance * 0.005f);
            }

            return 0f;
        }

    }

	public class RotomCritterNPCShiny : RotomCritterNPC{}
}