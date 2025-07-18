﻿using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MagnetonPet
{
	public class MagnetonPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [0,7];

		public override int[] idleFlyStartEnd => [0,7];
		public override int[] walkFlyStartEnd => [0,7];
		public override int[] attackFlyStartEnd => [8,15];
	}

	public class MagnetonPetProjectileShiny : MagnetonPetProjectile{}
}
