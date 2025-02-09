using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PidgeottoPet
{
	public class PidgeottoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 36;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [12,15];

		public override int[] idleFlyStartEnd => [8,11];
		public override int[] walkFlyStartEnd => [12,15];
		public override int[] attackFlyStartEnd => [12,15];

		public override float moveSpeed1 => 6;
        public override float moveSpeed2 => 10;

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 40;
		public override int attackCooldown => 90;

		public override string[] evolutions => ["Pidgeot"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WingAttack>(), GetPokemonDamage(60), 0f, Projectile.owner)];
						Projectile.velocity = 20*Vector2.Normalize(targetCenter-Projectile.Center);
						SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						isFlying = true;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			attackProjs[i].Center = Projectile.Center;
			if(Projectile.velocity.Length() < 1f){
				attackProjs[i].Kill();
				if(!canAttack){
					timer = 0;
				}
			}
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Center = Projectile.Center;
			if(Projectile.velocity.Length() < 1f){
				attackProjs[i].Kill();
				if(!canAttack){
					timer = 0;
				}
			}
		}

        public override void ExtraChanges()
        {
			if(!canAttack && timer > 0){
				immune = true;
			}
            base.ExtraChanges();
        }
	}

	public class PidgeottoPetProjectileShiny : PidgeottoPetProjectile{}
}