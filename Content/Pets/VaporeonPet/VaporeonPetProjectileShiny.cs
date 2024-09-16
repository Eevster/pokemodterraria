using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VaporeonPet
{
	public class VaporeonPetProjectileShiny : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 26;

		public override int nAttackProjs => 1;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<VaporeonPetBuffShiny>();
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
	}
}
