using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GolemPet
{
	public class GolemPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 60;
		public override int hitboxHeight => 56;

		public override int totalFrames => 12;
		public override int animationSpeed => 4;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [8,11];
	}

	public class GolemPetProjectileShiny : GolemPetProjectile{}
}
