using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class AlakazamCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 58;
		public override int hitboxHeight => 60;

		public override int totalFrames => 14;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,8];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [9, 9];
        public override int[] attackStartEnd => [9, 13];

        public override int minLevel => 40;
        public override float catchRate => 50;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Its superb memory lets it recall everything it has experienced from birth. Its IQ exceeds 5,000."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneForest)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.1f);
			}

			return 0f;
		}
		
	}

	public class AlakazamCritterNPCShiny : AlakazamCritterNPC{}
}