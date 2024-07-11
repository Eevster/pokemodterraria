using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BlastoisePet
{
	public class BlastoisePetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 0;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<BlastoisePetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;

		public override int totalFrames => 24;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,16];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [17,23];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 25;
			height = 70;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
