using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CyndaquilPet
{
	public class CyndaquilPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 26;

		public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,13];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 60;
		public override int attackCooldown => 60;

		public override string[] evolutions => ["Quilava"];
		public override int levelToEvolve => 14;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						float gravity = 0.25f;
						float targetDistanceY = -targetCenter.Y+Projectile.Center.Y;
						float throwSpeedY = System.Math.Clamp(targetDistanceY/20f,3f,30f);

						double delta = throwSpeedY*throwSpeedY-2f*gravity*targetDistanceY;

						if(delta<0){
							break;
						}

						double timeToReach = (float)(2f*throwSpeedY+(-throwSpeedY+System.Math.Sqrt(delta)))/gravity;
						float targetDistanceX = (float)(targetCenter.X-Projectile.Center.X);
						float throwSpeedX = (float)(targetDistanceX/timeToReach);

						Vector2 projSpeed = new Vector2(throwSpeedX,-throwSpeedY);

						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, projSpeed, ModContent.ProjectileType<Smokescreen>(), GetPokemonDamage(special: true), 0f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item20, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class CyndaquilPetProjectileShiny : CyndaquilPetProjectile{}
}
