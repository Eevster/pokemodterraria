using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PikachuPet
{
	public class PikachuPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 24;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override string[] evolutions => ["Raichu"];
		public override string[] itemToEvolve => ["ThunderStoneItem"];
	}

	public class PikachuPetProjectileShiny : PikachuPetProjectile{}
}
