using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ShellderPet
{
	public class ShellderPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,10];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [4, 4];
		public override int[] attackStartEnd => [11, 11];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,6];
		public override int[] walkSwimStartEnd => [7,10];
		public override int[] attackSwimStartEnd => [11,11];
		
        public override string[] evolutions => ["Cloyster"];
        public override string[] itemToEvolve => ["WaterStoneItem"];
	}

	public class ShellderPetProjectileShiny : ShellderPetProjectile{}
}
