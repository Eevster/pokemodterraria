using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenusaurPetMega
{
	public class VenusaurMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 64;
		public override int hitboxHeight => 52;

		public override int totalFrames => 25;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,14];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [15,24];

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;

		public override bool isMega => true;

		public override string[] megaEvolutionBase => ["Venusaur"];
		public override string[] itemToMegaEvolve => ["VenusaurMegaStoneItem"];
	}
	
	public class VenusaurMegaPetProjectileShiny : VenusaurMegaPetProjectile{}
}
