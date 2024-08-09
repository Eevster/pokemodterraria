using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.JolteonPet
{
	public class JolteonPetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 0;
		public override int baseDamage => 4;
		public override int PokemonBuff => ModContent.BuffType<JolteonPetBuff>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;
		public override bool canMoveWhileAttack => true;

		public override int totalFrames => 18;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [12,12];
		public override int[] fallStartEnd => [15,15];

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			//Projectile.width = 60;
			Projectile.width = 32;
			DrawOffsetX = -(28 - Projectile.width/2);
			Projectile.height = 40;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true; 
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						//attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Swift>(), GetPokemonDamage(), 2f, Projectile.owner, i*MathHelper.PiOver2)];
					}
				} 
			}
			timer = attackDuration;
			canAttack = false;
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			if(attackProjs[i].ai[1] == 0){
				attackProjs[i].Center = Projectile.position + new Vector2(25,23) + 50*new Vector2(1,0).RotatedBy(attackProjs[i].ai[0]);
			}
		}

		public override void UpdateNoAttackProjs(int i){
			if(attackProjs[i].ai[1] != 0){
				attackProjs[i] = null;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
			height = 32;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
