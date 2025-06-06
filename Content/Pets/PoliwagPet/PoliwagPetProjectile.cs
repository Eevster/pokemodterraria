using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PoliwagPet
{
	public class PoliwagPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 20;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [2,2];
		public override int[] attackStartEnd => [8,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [12, 15];
		public override int[] walkSwimStartEnd => [12,15];
		public override int[] attackSwimStartEnd => [8,11];

		public override string[] evolutions => ["Poliwhirl"];
		public override int levelToEvolve => 25;
		public override int levelEvolutionsNumber => 1;
	}

	public class PoliwagPetProjectileShiny : PoliwagPetProjectile{}
}
