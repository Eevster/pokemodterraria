using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RaichuPet
{
	public class RaichuPetProjectile : PokemonPetProjectile
	{
       
        public override int baseHP => 60;
        public override int baseDef => 90;
        public override int nAttackProjs => 6;
		public override int baseDamage => 4;
		public override int PokemonBuff => ModContent.BuffType<RaichuPetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 30;
		public override int attackCooldown => 60;
		public override bool canMoveWhileAttack => false;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

            currentHp = 1000;

            //Projectile.width = 96;
            Projectile.width = 24;
			DrawOffsetX = -(48 - Projectile.width/2);
			Projectile.height = 60;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0.5f;
			Projectile.tileCollide = true; 
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				if(currentStatus == (int)ProjStatus.Attack && timer > (attackDuration/2)){
					if(timer%5 == 4){
						for(int i = 0; i < nAttackProjs; i++){
							if(attackProjs[i] == null){
								attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter + new Vector2(i%3==0?0:(i%3==1?-32:32),-400), Vector2.Zero, ModContent.ProjectileType<ThunderCloud>(), GetPokemonDamage(), 2f, Projectile.owner)];
								SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
								break;
							}
						}
					}
				}else{
					canAttackOutTimer = false; 
				}
			}
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
