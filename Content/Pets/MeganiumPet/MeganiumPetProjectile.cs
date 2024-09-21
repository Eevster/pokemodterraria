using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MeganiumPet
{
	public class MeganiumPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;
		public override int[] baseStats => [80, 82, 100, 83, 100, 80];

		public override int totalFrames => 18;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,17];

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 56;
		public override int attackCooldown => 34;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<GigaDrain>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class MeganiumPetProjectileShiny : MeganiumPetProjectile{}
}
