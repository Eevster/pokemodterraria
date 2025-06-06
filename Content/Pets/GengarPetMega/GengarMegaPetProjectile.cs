using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GengarPetMega
{
	public class GengarMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 44;

		public override int moveStyle => 2;

        public override float distanceToFly => 300f;

        public override float moveDistance1 => 800f;
        public override float moveDistance2 => 500f;

        public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];

        public override int[] attackStartEnd => [8,11];

        public override int[] idleFlyStartEnd => [0,3];
        public override int[] walkFlyStartEnd => [4,7];
        public override int[] attackFlyStartEnd => [8,11];

		public override bool isMega => true;

		public override string[] megaEvolutionBase => ["Gengar"];
		public override string[] itemToMegaEvolve => ["GengarMegaStoneItem"];
	}
	
	public class GengarMegaPetProjectileShiny : GengarMegaPetProjectile{}
}
