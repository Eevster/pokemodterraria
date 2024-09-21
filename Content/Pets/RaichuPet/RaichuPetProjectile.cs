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
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 30;
		public override int[] baseStats => [60, 90, 55, 90, 80, 110];

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [15,15];
		public override int[] attackStartEnd => [18,18];

		public override int nAttackProjs => 6;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 30;
		public override int attackCooldown => 60;
		public override bool canMoveWhileAttack => false;

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
	}

	public class RaichuPetProjectileShiny : RaichuPetProjectile{}
}
