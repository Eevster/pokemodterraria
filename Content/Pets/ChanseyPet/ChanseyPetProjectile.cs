using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ChanseyPet
{
	public class ChanseyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 22;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [14,14];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,21];

		public override int nAttackProjs => 0;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 60;
		public override int attackCooldown => 45;
	}

	public class ChanseyPetProjectileShiny : ChanseyPetProjectile{}
}
