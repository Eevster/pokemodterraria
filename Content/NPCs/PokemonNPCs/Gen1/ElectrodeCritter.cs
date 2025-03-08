using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ElectrodeCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 36;

        public override float moveSpeed => 2.5f;

        public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override bool canRotate => true;

		public override int minLevel => 30;
		public override float catchRate => 60;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
				new FlavorTextBestiaryInfoElement("It is known to drift on winds if it is bloated to bursting with stored electricity."));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneMarble) {
                return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.5f);
			}

			return 0f;
		}
	}

	public class ElectrodeCritterNPCShiny : ElectrodeCritterNPC{}
}