using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TyphlosionPet
{
	public class TyphlosionPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 48;

		public override int totalFrames => 11;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [0,5];

		public override float moveDistance1 => 200f;
		public override float moveDistance2 => 140f;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }
	}

	public class TyphlosionPetProjectileShiny : TyphlosionPetProjectile{}
}
