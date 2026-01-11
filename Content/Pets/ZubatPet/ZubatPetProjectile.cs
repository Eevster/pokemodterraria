using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ZubatPet
{
	public class ZubatPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;
		public override int moveStyle => 1;

		public override int totalFrames => 6;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [0,2];
		
		public override int[] idleFlyStartEnd => [0,2];
		public override int[] walkFlyStartEnd => [0,2];
		public override int[] attackFlyStartEnd => [3,5];
		

		public override string[] evolutions => ["Golbat"];
		public override int levelToEvolve => 22;
		public override int levelEvolutionsNumber => 1;
	}

	public class ZubatPetProjectileShiny : ZubatPetProjectile{}
}
