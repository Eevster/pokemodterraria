using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ButterfreePet
{
	public class ButterfreePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 7;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [0,6];

		public override int[] idleFlyStartEnd => [0,6];
		public override int[] walkFlyStartEnd => [0,6];
		public override int[] attackFlyStartEnd => [0,6];

		public override int nAttackProjs => 0;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 45;
		public override int attackCooldown => 0;
	}

	public class ButterfreePetProjectileShiny : ButterfreePetProjectile{}
}
