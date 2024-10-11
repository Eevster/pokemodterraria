using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.HaunterPet
{
	public class HaunterPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 40;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];

		public override int nAttackProjs => 1;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 30;
		public override int attackCooldown => 90;

        public override bool tangible => false;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 0.2f;
        }

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						SoundEngine.PlaySound(SoundID.Item8, Projectile.position);
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<Hex>(), GetPokemonDamage(65, true), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
    }

	public class HaunterPetProjectileShiny : HaunterPetProjectile{}
}
