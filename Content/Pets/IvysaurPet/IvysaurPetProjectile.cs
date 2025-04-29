using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.IvysaurPet
{
	public class IvysaurPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 26;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,14];

		public override int maxJumpHeight => 6;

		public override string[] evolutions => ["Venusaur"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;
	}

	public class IvysaurPetProjectileShiny : IvysaurPetProjectile{}
}