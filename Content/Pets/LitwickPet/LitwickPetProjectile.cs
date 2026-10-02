using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LitwickPet
{
	public class LitwickPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 24;
        public override int hitboxHeight => 22;

        public override int moveStyle => 0;

       public override int totalFrames => 20;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [10,14];
		public override int[] walkStartEnd => [0,4];
		public override int[] jumpStartEnd => [15,18];
		public override int[] fallStartEnd => [18,19];
		public override int[] attackStartEnd => [5,9];

		public override string[] evolutions => ["Lampent"];
		public override int levelToEvolve => 41;
		public override int levelEvolutionsNumber => 1;

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.light = 1f;
        }
    }

	public class LitwickPetProjectileShiny : LitwickPetProjectile{}
}
