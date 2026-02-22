using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class AlolanSandslashCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 42;

        public override int totalFrames => 20;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 12];
        public override int[] jumpStartEnd => [10, 10];
        public override int[] fallStartEnd => [6, 6];
        public override int[] attackStartEnd => [13, 19];
        public override float catchRate => 90;
        public override int minLevel => 40;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneSnow) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.1f);
			}

			return 0f;
		}

		
	}

	public class AlolanSandslashCritterNPCShiny : AlolanSandslashCritterNPC { }
}