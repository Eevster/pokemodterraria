using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SquirtlePet
{
	public class SquirtlePetProjectileShiny : PokemonPetProjectile
	{
		public override int nAttackProjs => 8;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<SquirtlePetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 600f;
		public override bool canAttackThroughWalls => false;

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;

		public override int attackDuration => 10;
		public override int attackCooldown => 10;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [14,14];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,6];
		public override int[] walkSwimStartEnd => [7,13];
		public override int[] attackSwimStartEnd => [14,14];

		public override string[] evolutions => ["Wartortle"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			//Projectile.width = 44;
			Projectile.width = 24;
			Projectile.ignoreWater = false;
			DrawOffsetX = -(22 - Projectile.width/2);
			Projectile.height = 40;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 25f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<Bubble>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item54, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
			height = 32;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}