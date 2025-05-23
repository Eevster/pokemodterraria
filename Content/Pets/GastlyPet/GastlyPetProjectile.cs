﻿using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GastlyPet
{
	public class GastlyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 9;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [0,2];

		public override int[] idleFlyStartEnd => [0,2];
		public override int[] walkFlyStartEnd => [0,2];
		public override int[] attackFlyStartEnd => [3,8];

        public override bool tangible => false;

		public override string[] evolutions => ["Haunter"];
		public override int levelToEvolve => 25;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 0.2f;
        }
    }

	public class GastlyPetProjectileShiny : GastlyPetProjectile{}
}
