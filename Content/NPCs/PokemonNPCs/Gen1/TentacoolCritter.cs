using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class TentacoolCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,13];
		public override int[] jumpStartEnd => [9,12];
		public override int[] fallStartEnd => [4,8];
        public override int[] attackStartEnd => [14, 21];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [0, 5];
        public override int[] walkSwimStartEnd => [6, 13];
        public override int[] attackSwimStartEnd => [14, 21];
        public override float catchRate => 190;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) { 
			base.SetBestiary(database, bestiaryEntry);
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface);
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
				if (spawnInfo.Player.ZoneBeach) {
					return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.4f);
			}

			return 0f;
		}
		
	}

	public class TentacoolCritterNPCShiny : TentacoolCritterNPC{}
}