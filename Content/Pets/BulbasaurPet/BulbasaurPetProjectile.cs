using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BulbasaurPet
{
	public class BulbasaurPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,19];

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;

		public override int maxJumpHeight => 6;

		public override string[] evolutions => ["Ivysaur"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
	}

	public class BulbasaurPetProjectileShiny : BulbasaurPetProjectile{}
}
