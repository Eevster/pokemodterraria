using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MetapodPet
{
	public class MetapodPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 32;

		public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override float moveSpeed1 => 1;
        public override float moveSpeed2 => 2;

		public override int maxJumpHeight => 2;

        public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 64;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 40;
		public override int attackCooldown => 40;

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;

		public override string[] evolutions => ["Butterfree"];
		public override int levelToEvolve => 10;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Harden>(), GetPokemonDamage(20), 6f, Projectile.owner)];
						SoundEngine.PlaySound(SoundID.Item37, Projectile.position);
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			attackProjs[i].Center = Projectile.Center;
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Center = Projectile.Center;
		}
	}

	public class MetapodPetProjectileShiny : MetapodPetProjectile{}
}
