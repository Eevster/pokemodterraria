using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RaichuPetShiny
{
	public class RaichuPetProjectileShiny : PokemonPetProjectile
	{
		public override int nAttackProjs => 1;
		public override int baseDamage => 4;
		public override int PokemonBuff => ModContent.BuffType<RaichuPetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 30;
		public override int attackCooldown => 60;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 96;
			Projectile.height = 60;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0.5f;
			Projectile.tileCollide = true; 
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
			height = 36;
			hitboxCenterFrac = new Vector2(0.5f, 0.3f);
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
