using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MagnetonPet
{
	public class MagnetonPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 48;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [0,7];

		public override int[] idleFlyStartEnd => [0,7];
		public override int[] walkFlyStartEnd => [0,7];
		public override int[] attackFlyStartEnd => [8,15];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 40;
		public override int attackCooldown => 40;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<FlashCannon>(), GetPokemonDamage(80, true), 4f, Projectile.owner)];
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
			if(attackProjs[i].frame == 0){
				attackProjs[i].Center = Projectile.Center;
			}
		}

		public override void UpdateNoAttackProjs(int i){
			if(attackProjs[i].frame == 0){
				attackProjs[i].Center = Projectile.Center;
			}
		}
	}

	public class MagnetonPetProjectileShiny : MagnetonPetProjectile{}
}
