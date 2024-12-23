using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PidgeyPet
{
	public class PidgeyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 26;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [8,11];
		public override int[] attackFlyStartEnd => [12,15];

		public override int nAttackProjs => 0;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 40;
		public override int attackCooldown => 40;

		public override string[] evolutions => ["Pidgeotto"];
		public override int levelToEvolve => 18;
		public override int levelEvolutionsNumber => 1;
	}

	public class PidgeyPetProjectileShiny : PidgeyPetProjectile{}
}