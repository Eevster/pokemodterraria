using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MetapodPet
{
	public class MetapodPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override float moveSpeed1 => 1;
        public override float moveSpeed2 => 2;

        public override int nAttackProjs => 0;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 45;
		public override int attackCooldown => 0;
	}

	public class MetapodPetProjectileShiny : MetapodPetProjectile{}
}
