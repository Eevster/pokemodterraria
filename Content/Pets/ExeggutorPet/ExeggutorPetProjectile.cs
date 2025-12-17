using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ExeggutorPet
{
	public class ExeggutorPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 62;

		public override int totalFrames => 10;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0, 2];
		public override int[] walkStartEnd => [3, 8];
		public override int[] jumpStartEnd => [3, 3];
		public override int[] fallStartEnd => [6, 6];
		public override int[] attackStartEnd => [9, 9];
		

	}

	public class ExeggutorPetProjectileShiny : ExeggutorPetProjectile{}
}
