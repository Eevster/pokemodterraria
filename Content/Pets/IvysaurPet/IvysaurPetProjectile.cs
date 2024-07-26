using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.IvysaurPet
{
	public class IvysaurPetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 8;
		public override int baseDamage => 4;
		public override int PokemonBuff => ModContent.BuffType<IvysaurPetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 20;
		public override int attackCooldown => 20;
		public override bool canMoveWhileAttack => true;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,14];

		public override int maxJumpHeight => 6;

		public override string[] evolutions => ["Venusaur"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			//Projectile.width = 50;
			Projectile.width = 24;
			DrawOffsetX = -(25 - Projectile.width/2);
			Projectile.height = 42;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 8*new Vector2(0,-1).RotatedByRandom(MathHelper.ToRadians(20)), ModContent.ProjectileType<PoisonPowder>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item17, Projectile.position);
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
			height = 34;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}