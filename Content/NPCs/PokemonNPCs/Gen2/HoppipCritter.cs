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

        public override int totalFrames => 58;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 16];
        public override int[] walkStartEnd => [47, 57];
        public override int[] jumpStartEnd => [0, 0];
        public override int[] fallStartEnd => [17, 39];
        public override int[] attackStartEnd => [40, 47];
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