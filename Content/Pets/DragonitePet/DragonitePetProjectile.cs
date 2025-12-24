using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DragonitePet
{
	public class DragonitePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 64;

		public override int totalFrames => 24;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [16,19];
		public override int[] walkFlyStartEnd => [16,19];
		public override int[] attackFlyStartEnd => [20,23];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [16,19];
		public override int[] walkSwimStartEnd => [16,19];
		public override int[] attackSwimStartEnd => [20,23];
	}

	public class DragonitePetProjectileShiny : DragonitePetProjectile{}
}
