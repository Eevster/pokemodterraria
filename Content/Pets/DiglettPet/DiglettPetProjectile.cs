using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DiglettPet
{
	public class DiglettPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

		public override int totalFrames => 10;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];

        public override int maxJumpHeight => 2;

        public override string[] evolutions => ["Dugtrio"];
		public override int levelToEvolve => 20;
		public override int levelEvolutionsNumber => 1;
	}

	public class DiglettPetProjectileShiny : DiglettPetProjectile{}
}
