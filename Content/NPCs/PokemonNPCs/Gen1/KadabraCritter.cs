using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class KadabraCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 54;
		public override int hitboxHeight => 46;

		public override int totalFrames => 13;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,8];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [7,7];
        public override int[] attackStartEnd => [9, 12];

        public override int minLevel => 16;
        public override float catchRate => 50;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("When it uses its psychic power, it emits strong alpha waves that can ruin precision devices."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneForest)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Overworld.Chance * 0.3f);
			}

			return 0f;
		}
		
	}

	public class KadabraCritterNPCShiny : KadabraCritterNPC{}
}