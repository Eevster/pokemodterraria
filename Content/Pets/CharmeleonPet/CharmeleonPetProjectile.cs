using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharmeleonPet
{
	public class CharmeleonPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 40;

		public override int nAttackProjs => 1;
		public override int baseDamage => 4;
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 400f;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 90;
		public override int attackCooldown => 30;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,12];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [13,18];

		public override string[] evolutions => ["Charizard"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 20f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<Flamethrower>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			attackProjs[i].Center = Projectile.Center+new Vector2(0,-4);
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Center = Projectile.Center+new Vector2(0,-4);
		}
	}
	public class CharmeleonPetProjectileShiny : CharmeleonPetProjectile{}
}