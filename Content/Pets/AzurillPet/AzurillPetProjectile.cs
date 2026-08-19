using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AzurillPet
{
	public class AzurillPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 36;

		public override int totalFrames => 30;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [10,12];
		public override int[] fallStartEnd => [14,16];
        public override int[] attackStartEnd => [18,25];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [26,29];
		public override int[] walkSwimStartEnd => [26,29];
		public override int[] attackSwimStartEnd => [26,29];

		public override string[] evolutions => ["Marill"];
		public override string[] specialConditionToEvolve => ["Happiness"];

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
	}

	public class AzurillPetProjectileShiny : AzurillPetProjectile{}
}
