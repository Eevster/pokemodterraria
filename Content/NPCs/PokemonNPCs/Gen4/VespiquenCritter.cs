using Pokemod.Common.Configs;
using Pokemod.Common.UI;
using Pokemod.Content.Items.GeneticSamples;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class VespiquenCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 46;
        public override int hitboxHeight => 66;

        public override int totalFrames => 18;
        public override int animationSpeed => 8;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [0, 6];


        public override int[] idleFlyStartEnd => [0, 6];
        public override int[] walkFlyStartEnd => [0, 6];
        public override int[] attackFlyStartEnd => [7, 17];

        public override float catchRate => 45;
        public override int minLevel => 21;
        
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
                return GetSpawnChance(spawnInfo, 0.4f);
            }

            return 0f;
        }

    }

	public class VespiquenCritterNPCShiny : VespiquenCritterNPC{}
}