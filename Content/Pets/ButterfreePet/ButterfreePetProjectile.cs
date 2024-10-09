using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ButterfreePet
{
	public class ButterfreePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 36;

		public override int totalFrames => 7;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [0,6];

		public override int[] idleFlyStartEnd => [0,6];
		public override int[] walkFlyStartEnd => [0,6];
		public override int[] attackFlyStartEnd => [0,6];

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 56;
		public override int attackCooldown => 34;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item6, Projectile.position);
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<Confusion>(), GetPokemonDamage(50, true), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class ButterfreePetProjectileShiny : ButterfreePetProjectile{}
}
