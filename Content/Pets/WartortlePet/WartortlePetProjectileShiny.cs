using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WartortlePet
{
	public class WartortlePetProjectileShiny : PokemonPetProjectile
	{
        public override int baseHP => 59;
        public override int baseDef => 80;
        public override int nAttackProjs => 2;
		public override int baseDamage => 4;
		public override int PokemonBuff => ModContent.BuffType<WartortlePetBuffShiny>();
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => false;

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;
		
		public override int attackDuration => 40;
		public override int attackCooldown => 60;

		public override int totalFrames => 17;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,16];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,7];
		public override int[] walkSwimStartEnd => [8,15];
		public override int[] attackSwimStartEnd => [16,16];

		public override string[] evolutions => ["Blastoise"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.EyeOfCthulhuPet); // Copy the stats of the Suspicious Grinning Eye projectile

            currentHp = 1000;

            //Projectile.width = 60;
            Projectile.width = 32;
			DrawOffsetX = -(30 - Projectile.width/2);
			Projectile.height = 48;
			Projectile.aiStyle = -1; // Use custom AI
			Projectile.light = 0f;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
		}

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, 10f*Vector2.Normalize(targetCenter-Projectile.Center), ModContent.ProjectileType<WaterPulse>(), GetPokemonDamage(), 2f, Projectile.owner)];
						currentStatus = (int)ProjStatus.Attack;
						SoundEngine.PlaySound(SoundID.Item21, Projectile.position);
						timer = attackDuration;
						canAttack = false;
						break;
					}
				} 
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
			height = 40;
            fallThrough = false;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
	}
}