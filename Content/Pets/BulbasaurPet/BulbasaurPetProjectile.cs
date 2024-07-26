using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BulbasaurPet
{
	public class BulbasaurPetProjectile : PokemonPetProjectile
	{
		public override int nAttackProjs => 2;
		public override int baseDamage => 3;
		public override int PokemonBuff => ModContent.BuffType<BulbasaurPetBuff>();
		public override float enemySearchDistance => 1000;
		public override float distanceToAttack => 140f;
		public override bool canAttackThroughWalls => false;
		public override int attackDuration => 45;
		public override int attackCooldown => 0;

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;
		public override bool canMoveWhileAttack => true;

		public override int totalFrames => 20;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [11,19];

		public override int maxJumpHeight => 6;

		public override string[] evolutions => ["Ivysaur"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

			//Projectile.width = 44;
			Projectile.width = 24;
			DrawOffsetX = -(22 - Projectile.width/2);
			Projectile.height = 36;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0.3f;
			Projectile.tileCollide = true; 
		}
		
		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				if(attackProjs[0] == null || attackProjs[1] == null){
					currentStatus = (int)ProjStatus.Attack;
					SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
					if(attackProjs[0] == null) attackProjs[0] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipBack>(), GetPokemonDamage(), 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X))];
					if(attackProjs[1] == null) attackProjs[1] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<VineWhipFront>(), GetPokemonDamage(), 2f, Projectile.owner, 0, Math.Sign((targetCenter - Projectile.Center).X))];
				}
			}
			timer = attackDuration;
			canAttack = false;
		}

		public override void UpdateAttackProjs(int i, ref float maxFallSpeed){
			((PokemonAttack)attackProjs[i].ModProjectile).positionAux = Projectile.Center;
			attackProjs[i].netUpdate = true;
		}

		public override void UpdateNoAttackProjs(int i){
			attackProjs[i].Kill();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
			height = 26;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}
