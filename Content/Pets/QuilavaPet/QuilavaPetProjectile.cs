using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.QuilavaPet
{
	public class QuilavaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,11];

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 60;
		public override int attackCooldown => 90;

		public override string[] evolutions => ["Typhlosion"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlameWheel>(), GetPokemonDamage(60, true), 0f, Projectile.owner)];
						Projectile.velocity = 20*Vector2.Normalize(targetCenter-Projectile.Center);
						SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
						timer = attackDuration;
						canAttack = false;
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
	}

	public class QuilavaPetProjectileShiny : QuilavaPetProjectile{}
}
