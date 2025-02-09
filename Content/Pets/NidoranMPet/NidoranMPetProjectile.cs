using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.NidoranMPet
{
	public class NidoranMPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 24;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,9];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [10,15];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 200f;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 30;
		public override int attackCooldown => 60;

		public override string[] evolutions => ["Nidorino"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;

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
				if(currentStatus == (int)ProjStatus.Attack && Projectile.frame>=13 && Projectile.frame<=14 && timer%5 == 0){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<DoubleKick>(), GetPokemonDamage(30), 0f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
							break;
						}
					} 
				}
			}
		}
	}

	public class NidoranMPetProjectileShiny : NidoranMPetProjectile{}
}
