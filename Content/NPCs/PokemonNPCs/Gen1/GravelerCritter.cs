using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class GravelerCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 48;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [7, 7];
		public override int[] attackStartEnd => [8,11];

		public override float catchRate => 120;
		public override int minLevel => 25;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("It tumbles down slopes, heedless of any body parts chipping off. It eats a ton of rocks daily."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.ZoneNormalUnderground)
			{
				return GetSpawnChance(spawnInfo, SpawnCondition.Underground.Chance * 0.25f);
			}

			return 0f;
		}
		
	}

	public class GravelerCritterNPCShiny : GravelerCritterNPC{}
}