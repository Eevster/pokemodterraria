using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class LickilickyCritterNPC : PokemonWildNPC
	{
        public override int hitboxWidth => 40;
        public override int hitboxHeight => 52;

        public override int totalFrames => 31;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [8, 23];
        public override int[] walkStartEnd => [27, 30];
        public override int[] jumpStartEnd => [24, 26];
        public override int[] fallStartEnd => [24, 24];
        public override int[] attackStartEnd => [0, 7];
        public override float catchRate => 190;


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneForest) {
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.5f);
			}

			return 0f;
		}
		
	}

	public class LickilickyCritterNPCShiny : LickilickyCritterNPC{}
}