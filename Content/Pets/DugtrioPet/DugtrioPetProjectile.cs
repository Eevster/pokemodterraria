using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DugtrioPet
{
	public class DugtrioPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 32;

        public override int totalFrames => 14;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
		public override int[] attackStartEnd => [8,13];

		public override int maxJumpHeight => 2;
	}

	public class DugtrioPetProjectileShiny : DugtrioPetProjectile{}
}
