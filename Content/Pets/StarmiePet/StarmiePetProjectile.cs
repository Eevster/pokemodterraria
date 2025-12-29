using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.StarmiePet
{
	public class StarmiePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 32;

		public override int totalFrames => 13;
		public override int animationSpeed => 5;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,4];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [1,1];
        public override int[] attackStartEnd => [5,8];

		public override int[] idleFlyStartEnd => [9,12];
		public override int[] walkFlyStartEnd => [9,12];
		public override int[] attackFlyStartEnd => [5,8];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [9,12];
        public override int[] walkSwimStartEnd => [9,12];
		public override int[] attackSwimStartEnd => [5,8];
	}

	public class StarmiePetProjectileShiny : StarmiePetProjectile{}
}
