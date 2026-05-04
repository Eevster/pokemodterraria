using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DoduoPet
{
	public class AmpharosPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;

		public override int totalFrames => 30;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,10];
		public override int[] walkStartEnd => [24,29];
		public override int[] jumpStartEnd => [11,14];
		public override int[] fallStartEnd => [9,10];
        public override int[] attackStartEnd => [15, 23];

    }

	public class AmpharosPetProjectileShiny : AmpharosPetProjectile { }
}
