﻿using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BlastoisePet
{
	public class BlastoisePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 26;
		public override int hitboxHeight => 56;

		public override int totalFrames => 24;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,16];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [17,23];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,8];
		public override int[] walkSwimStartEnd => [9,16];
		public override int[] attackSwimStartEnd => [17,23];

		public override int nAttackProjs => 8;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 42;
		public override int attackCooldown => 18;


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
				if(currentStatus == (int)ProjStatus.Attack && Projectile.frame >= 21){
					int remainProjs = 2;
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center+new Vector2(Projectile.spriteDirection*(-18+50*(2-remainProjs)),-32), 20f*Vector2.Normalize(new Vector2(Projectile.spriteDirection*(2-remainProjs),-1)), ModContent.ProjectileType<HydroPump>(), GetPokemonDamage(110, true), 2f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
							remainProjs--;
							canAttackOutTimer = false;
							if(remainProjs <= 0){
								break;
							}
						}
					} 
				}
			}
		}
	}

	public class BlastoisePetProjectileShiny : BlastoisePetProjectile{}
}
