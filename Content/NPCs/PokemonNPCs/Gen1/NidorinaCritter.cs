using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class NidorinaCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 48;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [9, 9];
		public override int[] fallStartEnd => [6, 6];
		public override int[] attackStartEnd => [8, 11];

		public override int minLevel => 16;
		public override float catchRate => 120;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("When it senses danger, it raises all the barbs on its body. These barbs grow slower than Nidorino's."));
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

	public class NidorinaCritterNPCShiny : NidorinaCritterNPC{}
}