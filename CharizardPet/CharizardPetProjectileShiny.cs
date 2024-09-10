using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetProjectileShiny : PokemonPetProjectile
	{
        public override int baseHP => 78;
        public override int baseDef => 78;
        public override int nAttackProjs => 5;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<CharizardPetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 40;
		public override int attackCooldown => 40;

		public override int totalFrames => 42;
		public override int animationSpeed => 7;

		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [15,15];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [22,29];

		public override int[] idleFlyStartEnd => [16,21];
		public override int[] walkFlyStartEnd => [36,41];
		public override int[] attackFlyStartEnd => [30,35];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

            currentHp = 1000;

            //Projectile.width = 96;
            Projectile.width = 36;
			DrawOffsetX = -(48 - Projectile.width/2);
			Projectile.height = 80;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 1f;
			Projectile.tileCollide = true; 
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 25f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<FireBlast>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 36;
			height = 72;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        

    }
}