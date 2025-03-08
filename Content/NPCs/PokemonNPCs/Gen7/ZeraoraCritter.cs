using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace Pokemod.Content.NPCs.PokemonNPCs
{
	public class ZeraoraCritterNPC : PokemonWildNPC
	{
		public override int hitboxWidth => 64;
		public override int hitboxHeight => 64;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				new FlavorTextBestiaryInfoElement("Zeraora creates a powerful magnetic field by emitting strong electric currents from the pads on its hands and feet. Unlike most Electric-type Pokémon, Zeraora doesn’t have an organ within its body that can produce electricity. However, it is able to gather and store electricity from outside sources, then use it as its own electric energy."));
		}

		
	}

	public class ZeraoraCritterNPCShiny : ZeraoraCritterNPC{}
}