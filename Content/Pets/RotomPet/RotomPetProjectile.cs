using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RotomPet
{
	public class RotomPetProjectile : PokemonPetProjectile
	{
         public override int hitboxWidth => 28;
        public override int hitboxHeight => 42;

        public override int totalFrames => 19;
        public override int animationSpeed => 6;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 7];
        public override int[] walkStartEnd => [8, 13];
        public override int[] attackStartEnd => [14, 18];

        public override int[] idleFlyStartEnd => [0, 7];
        public override int[] walkFlyStartEnd => [8, 13];
        public override int[] attackFlyStartEnd => [14, 18];

        public override bool tangible => false;

		public override float moveSpeed1 => 10;
		public override float moveSpeed2 => 20;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 0.5f;
        }
    }

	public class RotomPetProjectileShiny : RotomPetProjectile{}
}
