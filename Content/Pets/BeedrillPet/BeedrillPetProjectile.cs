using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BeedrillPet
{
	public class BeedrillPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];
	}

	public class BeedrillPetProjectileShiny : BeedrillPetProjectile{}
}
