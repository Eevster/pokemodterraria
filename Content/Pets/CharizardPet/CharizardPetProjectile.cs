using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;

		public override int totalFrames => 42;
		public override int animationSpeed => 7;

		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [15,15];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [22,29];

		public override int[] idleFlyStartEnd => [16,21];
		public override int[] walkFlyStartEnd => [36,41];
		public override int[] attackFlyStartEnd => [30,35];

		public override float distanceToFly => 300f;

		public override int nAttackProjs => 5;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 40;
		public override int attackCooldown => 80;

        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }

        public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 25f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<FireBlast>(), GetPokemonDamage(110, true), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}
	}

	public class CharizardPetProjectileShiny : CharizardPetProjectile{}
}