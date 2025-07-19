using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GengarPet
{
	public class GengarPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int moveStyle => 2;

        public override float distanceToFly => 300f;

		public override float moveDistance1 => 800f;
		public override float moveDistance2 => 500f;

		public override int totalFrames => 18;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,9];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [7,7];

		public override int[] attackStartEnd => [10, 13];

		public override int[] idleFlyStartEnd => [14, 17];
		public override int[] walkFlyStartEnd => [14, 17];
		public override int[] attackFlyStartEnd => [10, 13];
		
		public override string[] megaEvolutions => ["GengarMega"];
		public override string[] itemToMegaEvolve => ["GengarMegaStoneItem"];
	}

	public class GengarPetProjectileShiny : GengarPetProjectile{}
}
