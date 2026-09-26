using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.BreloomPet
{
	public class BreloomPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 24;
		public override int hitboxHeight => 42;

		public override int totalFrames => 24;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [13,17];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [18,23];
		public override int[] fallStartEnd => [22,23];
        public override int[] attackStartEnd => [6, 12];
    }

	public class BreloomPetProjectileShiny : BreloomPetProjectile{ }
}
