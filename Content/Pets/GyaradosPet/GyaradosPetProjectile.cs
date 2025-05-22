using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GyaradosPet
{
	public class GyaradosPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 118;
		public override int hitboxHeight => 100;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,9];
		public override int[] attackStartEnd => [10, 14];

		public override int[] idleFlyStartEnd => [0, 3];
		public override int[] walkFlyStartEnd => [4, 9];
		public override int[] attackFlyStartEnd => [10, 14];

		public override string[] megaEvolutions => ["GyaradosMega"];
		public override string[] itemToMegaEvolve => ["GyaradosMegaStoneItem"];
	}

	public class GyaradosPetProjectileShiny : GyaradosPetProjectile{}
}
