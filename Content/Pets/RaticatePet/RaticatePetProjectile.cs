using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RaticatePet
{
	public class RaticatePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [0,3];

		public override int nAttackProjs => 3;
		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 400f;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 42;
		public override int attackCooldown => 20;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<HyperFang>(), GetPokemonDamage(80), 0, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class RaticatePetProjectileShiny : RaticatePetProjectile{}
}
