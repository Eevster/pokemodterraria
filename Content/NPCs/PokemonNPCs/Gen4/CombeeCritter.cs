using Pokemod.Common.Configs;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Pokemod.Common.UI;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class CombeeCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 38;
        public override int hitboxHeight => 30;

        public override int totalFrames => 4;
        public override int animationSpeed => 8;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 1];
        public override int[] walkStartEnd => [0, 1];

        public override int[] idleFlyStartEnd => [0, 1];
        public override int[] walkFlyStartEnd => [0, 1];
        public override int[] attackFlyStartEnd => [2, 3];

        public override float catchRate => 120;
        
        public override int[][] spawnConditions =>
        [
            [(int)SpawnArea.Surface, (int)DayTimeStatus.All, (int)WeatherStatus.All]
        ];


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {

            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle);
            base.SetBestiary(database, bestiaryEntry);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneJungle)
            {
                return GetSpawnChance(spawnInfo, 1.0f);
            }

            return 0f;
        }

    }

	public class CombeeCritterNPCShiny : CombeeCritterNPC{}
}