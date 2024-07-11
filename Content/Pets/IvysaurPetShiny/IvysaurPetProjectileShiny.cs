using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.IvysaurPetShiny
{
	public class IvysaurPetProjectileShiny : PokemonPetProjectile
	{
		public override int nAttackProjs => 0;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<IvysaurPetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,14];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 50;
			Projectile.height = 42;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
			height = 34;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}