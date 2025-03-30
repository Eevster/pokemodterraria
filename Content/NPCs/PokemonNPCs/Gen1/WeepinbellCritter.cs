using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Pokemod.Common.Configs;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class WeepinbellCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 30;
		public override int hitboxHeight => 28;

		public override int totalFrames => 8;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [3,3];
		public override int[] attackStartEnd => [4,7];

		public override int minLevel => 25;
		public override float catchRate => 120;

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				new FlavorTextBestiaryInfoElement("This Pokémon has the appearance of a plant. It captures unwary prey by dousing them with a toxic powder."));
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			if (spawnInfo.Player.ZoneJungle) {
				return GetSpawnChance(spawnInfo, (SpawnCondition.Overworld.Chance+SpawnCondition.Underground.Chance) * 0.2f);
			}

			return 0f;
		}
		
	}

	public class WeepinbellCritterNPCShiny : WeepinbellCritterNPC{}
}