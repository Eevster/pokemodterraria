using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.FeraligatrPet
{
	public class FeraligatrPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 50;

		public override int totalFrames => 18;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [12,17];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [6,11];
		public override int[] attackSwimStartEnd => [12,17];

		public override int nAttackProjs => 6;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 600f;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 42;
		public override int attackCooldown => 20;

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
				if(currentStatus == (int)ProjStatus.Attack && Projectile.frame >= 15){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter + new Vector2(0,22), new Vector2(0,-10), ModContent.ProjectileType<Waterfall>(), GetPokemonDamage(80), 6f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
							canAttackOutTimer = false; 
							break;
						}
					}
				}
			}
		}
	}

	public class FeraligatrPetProjectileShiny : FeraligatrPetProjectile{}
}
