using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RattataPet
{
	public class RattataPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 20;

		public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];

		public override int nAttackProjs => 0;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 60;
		public override int attackCooldown => 45;
	}

	public class RattataPetProjectileShiny : RattataPetProjectile{}
}
