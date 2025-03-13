using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace Pokemod.Content.Pets.WeezingPet
{
	public class WeezingPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 56;
		public override int hitboxHeight => 40;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;
        public override bool sideDiff => true;

        public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] attackStartEnd => [1,1];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [1,1];

		public override int nAttackProjs => 3;
		public override float enemySearchDistance => 1000;
		public override bool canAttackThroughWalls => true;
		public override int attackDuration => 0;
		public override int attackCooldown => 120;
		public override bool canMoveWhileAttack => true;

		public override void Attack(float distanceFromTarget, Vector2 targetCenter){
			SoundEngine.PlaySound(SoundID.Item4, Projectile.position);
			if(Projectile.owner == Main.myPlayer){
				for(int i = 0; i < nAttackProjs; i++){
					if(attackProjs[i] == null){
						attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<Swift>(), GetPokemonDamage(60, true), 2f, Projectile.owner, i*MathHelper.TwoPi/3)];
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

		public override void ExtraChanges() {
			if(Main.rand.NextBool(10)){
				int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(220, 223), 1f);
				Main.gore[goreIndex].scale = 0.5f;
				Main.gore[goreIndex].position = Projectile.position + 0.5f*hitboxWidth*Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
				Main.gore[goreIndex].velocity = 0.05f*hitboxWidth*(Main.gore[goreIndex].position-Projectile.position).SafeNormalize(Vector2.UnitX);
			}
		}
	}

	public class WeezingPetProjectileShiny : WeezingPetProjectile{}
}
