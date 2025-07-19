using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharizardPetMegaX
{
	public class CharizardMegaXPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 36;
        public override int hitboxHeight => 46;

        public override int totalFrames => 42;
		public override int animationSpeed => 7;

		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [15,15];
		public override int[] fallStartEnd => [12,12];
		public override int[] attackStartEnd => [22,29];

		public override int[] idleFlyStartEnd => [16,21];
		public override int[] walkFlyStartEnd => [36,41];
		public override int[] attackFlyStartEnd => [30,35];

		public override float distanceToFly => 300f;

		public override float moveDistance1 => 800f;
		public override float moveDistance2 => 500f;

		public override float moveSpeed1 => 7;
        public override float moveSpeed2 => 10;

        public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Charizard"];
		public override string[] itemToMegaEvolve => ["CharizardMegaStoneItemX"];

        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 1f;
        }
	}

	public class CharizardMegaXPetProjectileShiny : CharizardMegaXPetProjectile{}
}