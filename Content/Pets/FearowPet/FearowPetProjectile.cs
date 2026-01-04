using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.FearowPet
{
	public class FearowPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 54;

		public override int totalFrames => 16;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [8,11];
		public override int[] attackFlyStartEnd => [12,15];
	}

	public class FearowPetProjectileShiny : FearowPetProjectile{}
}
