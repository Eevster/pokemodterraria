using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MetapodPet
{
	public class MetapodPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 28;

        public override int totalFrames => 5;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [0,0];

        public override float moveSpeed1 => 1;
        public override float moveSpeed2 => 2;

		public override int maxJumpHeight => 2;

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;

		public override string[] evolutions => ["Butterfree"];

		public override int levelToEvolve => 10;
		public override int levelEvolutionsNumber => 1;
	}

	public class MetapodPetProjectileShiny : MetapodPetProjectile{}
}
