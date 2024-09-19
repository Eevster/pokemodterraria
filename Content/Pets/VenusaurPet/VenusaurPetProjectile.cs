using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenusaurPet
{
	public class VenusaurPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 50;
		public override int hitboxHeight => 40;

		public override int nAttackProjs => 1;
		public override int baseDamage => 5;
		public override float enemySearchDistance => 1500;
		public override bool canAttackThroughWalls => true;

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;
		
		public override int attackDuration => 70;
		public override int attackCooldown => 30;

		public override int totalFrames => 25;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,14];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [14,14];
		public override int[] attackStartEnd => [15,24];

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center+new Vector2(0,-28), Vector2.Zero, ModContent.ProjectileType<SolarBeamHold>(), GetPokemonDamage(), 4f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item43, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			attackProjs[i].Center = Projectile.Center+new Vector2(0,-28);
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Center = Projectile.Center+new Vector2(0,-28);
		}
	}

	public class VenusaurPetProjectileShiny : VenusaurPetProjectile{}
}