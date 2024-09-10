using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VaporeonPet
{
	public class VaporeonPetProjectile : PokemonPetProjectile
	{
       
        public override int baseHP => 130;
        public override int baseDef => 60;
        public override int nAttackProjs => 1;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<VaporeonPetBuff>();
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 100f;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 90;
		public override int attackCooldown => 60;
		public override bool canMoveWhileAttack => false;

		public override float moveDistance1 => 50f;
		public override float moveDistance2 => 50f;

		public override int totalFrames => 19;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,17];
		public override int[] attackSwimStartEnd => [18,18];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

            currentHp = 1000;
            //Projectile.width = 60;
            Projectile.width = 32;
			DrawOffsetX = -(40 - Projectile.width/2);
			Projectile.height = 44;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AquaRing>(), GetPokemonDamage(), 4f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			attackProjs[i].Center = Projectile.Center;
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Center = Projectile.Center;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
			height = 36;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
       
        

    }
}
