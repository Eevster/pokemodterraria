using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PichuPet
{
	public class PichuPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,8];
		public override int[] walkStartEnd => [9,17];
		public override int[] jumpStartEnd => [10,12];
		public override int[] fallStartEnd => [13,15];
		public override int[] attackStartEnd => [18,25];

		public override string[] evolutions => ["Pichu"];
		public override string[] specialConditionToEvolve => ["Happiness"];

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
	}

	public class PichuPetProjectileShiny : PichuPetProjectile{}
}
