using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharmeleonPet
{
	public class CharmeleonPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 40;

		public override int totalFrames => 19;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,12];
		public override int[] jumpStartEnd => [10,10];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [13,18];

		public override string[] evolutions => ["Charizard"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }

        public override void ExtraChanges()
        {
            if (variant == "Christmas")
            {
                ChangeAttackColor(new Color(21, 40, 255));
            }
            base.ExtraChanges();
        }
    }
	public class CharmeleonPetProjectileShiny : CharmeleonPetProjectile{}
}