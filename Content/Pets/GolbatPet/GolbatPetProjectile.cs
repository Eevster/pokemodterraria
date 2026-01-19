using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GolbatPet
{
	public class GolbatPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 46;

		public override int moveStyle => 1;

		public override int totalFrames => 4;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [0,2];
		
		public override int[] idleFlyStartEnd => [0,2];
		public override int[] walkFlyStartEnd => [0,2];
		public override int[] attackFlyStartEnd => [3,3];
	}

	public class GolbatPetProjectileShiny : GolbatPetProjectile{}
}
