﻿using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ElectrodePet
{
	public class ElectrodePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 34;

        public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override bool canRotate => true;

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;

		public override float moveSpeed1 => 8;
        public override float moveSpeed2 => 12;
	}

	public class ElectrodePetProjectileShiny : ElectrodePetProjectile{}
}
