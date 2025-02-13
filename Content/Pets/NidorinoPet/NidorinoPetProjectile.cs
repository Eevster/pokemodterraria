using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.NidorinoPet
{
	public class NidorinoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 30;

		public override int totalFrames => 16;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,9];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [10,15];

        public override string[] evolutions => ["Nidoking"];
        public override string[] itemToEvolve => ["MoonStoneItem"];

        public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 36;
		public override int attackCooldown => 64;
		public override bool canMoveWhileAttack => true;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						currentStatus = (int)ProjStatus.Attack;
						timer = attackDuration;
						canAttack = false;
						canAttackOutTimer = true;
						break;
					}
				} 
			}
		}

		public override void AttackOutTimer(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				if(currentStatus == (int)ProjStatus.Attack && Projectile.frame>=13){
					for(int i = 0; i < nAttackProjs; i++){
						if(attackProjs[i] == null){
							attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<Toxic>(), GetPokemonDamage(special: true), 0f, Projectile.owner)];
							SoundEngine.PlaySound(SoundID.Drown, Projectile.position);
							for (int k = 0; k < 40; k++)
							{
								int dustIndex = Dust.NewDust(Projectile.Center-0.5f*new Vector2(Projectile.width, Projectile.height), Projectile.width, Projectile.height, DustID.Venom, 0, -4, 100, default(Color), 1f);
								Main.dust[dustIndex].scale = 1f + (float)Main.rand.Next(3) * 0.2f;
								Main.dust[dustIndex].fadeIn = 1f + (float)Main.rand.Next(5) * 0.1f;
								Main.dust[dustIndex].noGravity = true;
							}
							canAttackOutTimer = false;
							break;
						}
					} 
				}
			}
		}
	}

	public class NidorinoPetProjectileShiny : NidorinoPetProjectile{}
}
