using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AbraPet
{
	public class AbraPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 64;
		public override int hitboxHeight => 64;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];

		public override string[] evolutions => ["Kadabra"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;

        public override int nAttackProjs => 1;
        public override float enemySearchDistance => 1000;
        public override bool canAttackThroughWalls => false;
        public override int attackDuration => 56;
        public override int attackCooldown => 34;

        public override void Attack(float distanceFromTarget, Vector2 targetCenter)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                for (int i = 0; i < nAttackProjs; i++)
                {
                    if (attackProjs[i] == null)
                    {
                        SoundEngine.PlaySound(SoundID.Item6 with { Volume = 0.5f }, Projectile.position);
                        attackProjs[i] = Main.projectile[Projectile.NewProjectile(Projectile.InheritSource(Projectile), targetCenter, Vector2.Zero, ModContent.ProjectileType<Confusion>(), GetPokemonDamage(50, true), 2f, Projectile.owner)];
                        currentStatus = (int)ProjStatus.Attack;
                        timer = attackDuration;
                        canAttack = false;
                        break;
                    }
                }
            }
        }
    }

	public class AbraPetProjectileShiny : AbraPetProjectile{}
}
