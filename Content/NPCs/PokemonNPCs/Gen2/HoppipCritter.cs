using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
    public class HoppipCritterNPC : PokemonWildNPC
    {
        public override int hitboxWidth => 92;
        public override int hitboxHeight => 60;

        public override int totalFrames => 61;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [33, 50];
        public override int[] walkStartEnd => [50, 60];
        public override int[] jumpStartEnd => [35, 41];
        public override int[] fallStartEnd => [8, 31];
        public override int[] attackStartEnd => [0, 8];
        public override float catchRate => 180;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            base.SetBestiary(database, bestiaryEntry);
            bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneForest)
            {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.9f);
            }

            return 0f;
        }


    }

    public class HoppipCritterNPCShiny : HoppipCritterNPC { }
}