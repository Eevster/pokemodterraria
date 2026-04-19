using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
    public class SkiploomCritterNPC : PokemonWildNPC
    {
        public override int hitboxWidth => 58;
        public override int hitboxHeight => 40;

        public override int totalFrames => 44;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [22, 35];
        public override int[] walkStartEnd => [35, 43];
        public override int[] jumpStartEnd => [8, 14];
        public override int[] fallStartEnd => [8, 14];
        public override int[] attackStartEnd => [0, 22];
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

    public class SkiploomCritterNPCShiny : SkiploomCritterNPC { }
}