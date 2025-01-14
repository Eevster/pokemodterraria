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

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;
		public override bool canMoveWhileAttack => true;

		public override string[] evolutions => ["Pidgeot"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Swift>(), GetPokemonDamage(60, true), 2f, Projectile.owner, i*MathHelper.TwoPi/3)];
					}
				} 
			}
			timer = attackDuration;
			canAttack = false;
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			if(attackProjs[i].ai[1] == 0){
				attackProjs[i].Center = Projectile.position + new Vector2(25,23) + 50*new Vector2(1,0).RotatedBy(attackProjs[i].ai[0]);
			}
		}

		public override void UpdateNoAttackProjs(int i){
			if(attackProjs[i].ai[1] != 0){
				attackProjs[i] = null;
			}
		}
	}

	public class PidgeottoPetProjectileShiny : PidgeottoPetProjectile{}
}