using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PidgeottoPet
{
	public class PidgeottoPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 36;

        public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [12,15];
		public override int[] attackFlyStartEnd => [12,15];

		public override float moveSpeed1 => 6;
        public override float moveSpeed2 => 10;

		public override string[] evolutions => ["Pidgeot"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;
	}

	public class PidgeottoPetProjectileShiny : PidgeottoPetProjectile{}
}