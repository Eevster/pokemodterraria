using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ClodsireCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 82;
        public override int hitboxHeight => 38;

        public override int totalFrames => 49;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [13, 27];
        public override int[] walkStartEnd => [8, 12];
        public override int[] jumpStartEnd => [8, 12];
        public override int[] fallStartEnd => [10, 10];
        public override int[] attackStartEnd => [0, 7];
        public override bool canSwim => true;
        public override int[] idleSwimStartEnd => [33, 40];
        public override int[] walkSwimStartEnd => [40, 48];
        public override int[] attackSwimStartEnd => [28, 32];
        public override float catchRate => 90;
        public override int minLevel => 32;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneSnow) {
                return GetSpawnChance(spawnInfo, SpawnCondition.OverworldDay.Chance * 0.09f);
			}

			return 0f;
		}

		
	}

	public class ClodsireCritterNPCShiny : ClodsireCritterNPC { }
}