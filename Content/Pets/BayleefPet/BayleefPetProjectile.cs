using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BayleefPet
{
	public class BayleefPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 40;
		public override int[] baseStats => [60, 62, 80, 63, 80, 60];

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,11];

		public override int nAttackProjs => 8;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 10;
		public override int attackCooldown => 20;

		public override string[] evolutions => ["Meganium"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, new Vector2(Main.rand.NextFloat(-10,10),-10), ModContent.ProjectileType<MagicalLeaf>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class BayleefPetProjectileShiny : BayleefPetProjectile{}
}
