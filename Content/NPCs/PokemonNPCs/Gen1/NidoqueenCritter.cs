using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class NidoqueenCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 72;
		public override int hitboxHeight => 68;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [6, 6];
		public override int[] attackStartEnd => [8, 11];

        public override int minLevel => 40;
        public override float catchRate => 50;

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Its entire body is armored with hard scales. It will protect the young in its burrow with its life."));
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

	public class NidoqueenCritterNPCShiny : NidoqueenCritterNPC{}
}