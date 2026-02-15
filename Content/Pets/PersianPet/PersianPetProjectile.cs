using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PersianPet
{
	public class PersianPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 13;
		public override int animationSpeed => 9;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,11];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [12, 12];
	}

	public class PersianPetProjectileShiny : PersianPetProjectile{}
}
