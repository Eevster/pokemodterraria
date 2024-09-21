using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CyndaquilPet
{
	public class CyndaquilPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 26;
		public override int[] baseStats => [39, 52, 43, 60, 50, 65];

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,13];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 45;
		public override int attackCooldown => 0;
	}

	public class CyndaquilPetProjectileShiny : CyndaquilPetProjectile{}
}
