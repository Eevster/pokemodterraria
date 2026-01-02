using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MawilePet
{
	public class MawilePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 30;

		public override int totalFrames => 19;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,9];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [8,8];
        public override int[] attackStartEnd => [10, 18];
	}

	public class MawilePetProjectileShiny : MawilePetProjectile{}
}