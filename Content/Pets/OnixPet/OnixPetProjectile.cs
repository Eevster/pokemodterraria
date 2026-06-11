using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.OnixPet
{
	public class OnixPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 92;
		public override int hitboxHeight => 78;

		public override int totalFrames => 5;
		public override int animationSpeed => 15;
		public override int[] idleStartEnd => [0, 1];
		public override int[] walkStartEnd => [0, 3];
		public override int[] jumpStartEnd => [0, 0];
		public override int[] fallStartEnd => [1, 1];
		public override int[] attackStartEnd => [4, 4];
	}

	public class OnixPetProjectileShiny : OnixPetProjectile{}
}
