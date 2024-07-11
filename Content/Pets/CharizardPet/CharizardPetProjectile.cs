using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 0;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<CharizardPetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;

		public override int totalFrames => 36;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [15,15];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [22,29];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			Projectile.width = 96;
			Projectile.height = 80;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 50;
			height = 72;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}