using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CroconawPet
{
	public class CroconawPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 38;

		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [12,17];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [6,11];
		public override int[] attackSwimStartEnd => [12,17];

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 200f;
		public override bool canAttackThroughWalls => false;

		public override int attackDuration => 30;
		public override int attackCooldown => 60;

		public override string[] evolutions => ["Feraligatr"];
		public override int levelToEvolve => 30;
		public override int levelEvolutionsNumber => 1;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter - 18*Vector2.Normalize(targetCenter-Projectile.Center), Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<IceFang>(), GetPokemonDamage(65), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class CroconawPetProjectileShiny : CroconawPetProjectile{}
}
