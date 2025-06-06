using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenusaurPet
{
	public class VenusaurPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 50;
		public override int hitboxHeight => 40;

		public override int totalFrames => 25;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0, 5];
		public override int[] walkStartEnd => [6, 14];
		public override int[] jumpStartEnd => [10, 10];
		public override int[] fallStartEnd => [14, 14];
		public override int[] attackStartEnd => [15, 24];

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;
		
		public override string[] megaEvolutions => ["VenusaurMega"];
		public override string[] itemToMegaEvolve => ["VenusaurMegaStoneItem"];
	}

	public class VenusaurPetProjectileShiny : VenusaurPetProjectile{}
}