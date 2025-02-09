using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ChanseyPet
{
	public class ChanseyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 22;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [14,14];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,21];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 200f;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 30;
		public override int attackCooldown => 90;

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
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HealPulse>(), GetPokemonDamage(special: true), 2f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
							canAttackOutTimer = false;
							break;
						}
					} 
				}
			}
		}
	}

	public class ChanseyPetProjectileShiny : ChanseyPetProjectile{}
}
