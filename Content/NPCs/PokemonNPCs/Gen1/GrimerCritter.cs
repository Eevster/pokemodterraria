using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GrimerCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 28;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [11,19];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [20,23];
		public override int[] fallStartEnd => [24,24];
		public override int[] attackStartEnd => [9,10];

		public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
        }
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (ModContent.GetInstance<BetaMonsConfig>().BetaMonsToggle) {
                if (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson)
                {
                    return GetSpawnChance(spawnInfo, (SpawnCondition.Crimson.Chance + SpawnCondition.Corruption.Chance) * 0.5f);
                }
            }

			return 0f;
		}
		
	}

	public class GrimerCritterNPCShiny : GrimerCritterNPC{}
}