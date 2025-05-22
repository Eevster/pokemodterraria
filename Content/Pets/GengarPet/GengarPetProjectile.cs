using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GengarPet
{
	public class GengarPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 56;
		public override int hitboxHeight => 60;

        public override int moveStyle => 2;

        public override float distanceToFly => 300f;

        public override float moveDistance1 => 800f;
        public override float moveDistance2 => 500f;

        public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [5,5];

        public override int[] attackStartEnd => [8, 11];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [0, 3];
        public override int[] attackFlyStartEnd => [8, 11];
	}

	public class GengarPetProjectileShiny : GengarPetProjectile{}
}
