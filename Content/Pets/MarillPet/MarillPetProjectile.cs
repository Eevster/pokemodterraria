using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MarillPet
{
	public class MarillPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 24;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [9,11];
		public override int[] fallStartEnd => [13,15];
        public override int[] attackStartEnd => [18,25];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,17];
		public override int[] attackSwimStartEnd => [18,25];

		public override string[] evolutions => ["Azumarill"];
		public override int levelToEvolve => 18;
		public override int levelEvolutionsNumber => 1;
	}

	public class MarillPetProjectileShiny : MarillPetProjectile{}
}
