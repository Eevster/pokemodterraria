using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.UnownPet
{
	public class UnownPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 14;
		public override int hitboxHeight => 22;

        public override int moveStyle => 1;
		
		public override int totalFrames => 1;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [0,0];
	}

	public class UnownPetProjectileShiny : UnownPetProjectile{}
}
