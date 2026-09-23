using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SquirtlePet
{
	public class SquirtlePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;

		public override int totalFrames => 39;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] runStartEnd => [14,19];
		public override int[] jumpStartEnd => [6,8];
		public override int[] fallStartEnd => [9,10];
		public override int[] attackStartEnd => [20,25];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [26,31];
		public override int[] walkSwimStartEnd => [32,38];
		public override int[] attackSwimStartEnd => [20,25];

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;

		public override string[] evolutions => ["Wartortle"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
	}

	public class SquirtlePetProjectileShiny : SquirtlePetProjectile{}
}
