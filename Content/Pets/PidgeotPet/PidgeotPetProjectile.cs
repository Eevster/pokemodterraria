using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PidgeotPet
{
	public class PidgeotPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 52;

		public override int totalFrames => 16;
		public override int animationSpeed => 7;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8,11];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [12,15];
		public override int[] attackFlyStartEnd => [8,11];

		public override float moveSpeed1 => 8;
        public override float moveSpeed2 => 12;

		public override int nAttackProjs => 10;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 80;
		public override int attackCooldown => 40;

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
				if(currentStatus == (int)ProjStatus.Attack && timer%10 == 0){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 22f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<AirSlash>(), GetPokemonDamage(75, true), 2f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
							break;
						}
					} 
				}
			}
		}
	}

	public class PidgeotPetProjectileShiny : PidgeotPetProjectile{}
}