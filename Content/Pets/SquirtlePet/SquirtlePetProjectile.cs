using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SquirtlePet
{
	public class SquirtlePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;
		public override int[] baseStats => [44, 48, 65, 50, 64, 43];

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [14,14];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,6];
		public override int[] walkSwimStartEnd => [7,13];
		public override int[] attackSwimStartEnd => [14,14];

		public override int nAttackProjs => 8;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 600f;
		public override bool canAttackThroughWalls => false;

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;

		public override int attackDuration => 10;
		public override int attackCooldown => 10;

		public override string[] evolutions => ["Wartortle"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 25f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<Bubble>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class SquirtlePetProjectileShiny : SquirtlePetProjectile{}
}
