using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DragonairPet
{
	public class DragonairPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 64;

		public override int totalFrames => 23;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,10];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11, 14];

		public override int[] idleFlyStartEnd => [15,18];
		public override int[] walkFlyStartEnd => [15,18];
		public override int[] attackFlyStartEnd => [19,22];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,3];
		public override int[] walkSwimStartEnd => [4,10];
		public override int[] attackSwimStartEnd => [11, 14];

		public override string[] evolutions => ["Dragonite"];
		public override int levelToEvolve => 55;
		public override int levelEvolutionsNumber => 1;
	}

	public class DragonairPetProjectileShiny : DragonairPetProjectile{}
}
