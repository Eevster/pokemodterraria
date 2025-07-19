using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BlastoisePetMega
{
	public class BlastoiseMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 52;
		public override int hitboxHeight => 46;

		public override int totalFrames => 24;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,16];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [17,23];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,16];
		public override int[] attackSwimStartEnd => [17,23];

		public override bool isMega => true;

		public override string[] megaEvolutionBase => ["Blastoise"];
		public override string[] itemToMegaEvolve => ["BlastoiseMegaStoneItem"];
	}
	
	public class BlastoiseMegaPetProjectileShiny : BlastoiseMegaPetProjectile{}
}
