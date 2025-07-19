using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KoffingPet
{
	public class KoffingPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 32;

        public override int totalFrames => 2;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] attackStartEnd => [1,1];

		public override int[] idleFlyStartEnd => [0,0];
		public override int[] walkFlyStartEnd => [0,0];
		public override int[] attackFlyStartEnd => [1,1];

		public override string[] evolutions => ["Weezing"];
		public override int levelToEvolve => 35;
		public override int levelEvolutionsNumber => 1;

		public override void ExtraChanges() {
            if (!Projectile.hide)
            {
                if (Main.rand.NextBool(10))
                {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, Main.rand.Next(220, 223), 1f);
                    Main.gore[goreIndex].scale = 0.5f;
                    Main.gore[goreIndex].position = Projectile.position + 0.5f * hitboxWidth * Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    Main.gore[goreIndex].velocity = 0.05f * hitboxWidth * (Main.gore[goreIndex].position - Projectile.position).SafeNormalize(Vector2.UnitX);
                }
            }
            base.ExtraChanges();
		}
	}

	public class KoffingPetProjectileShiny : KoffingPetProjectile{}
}
