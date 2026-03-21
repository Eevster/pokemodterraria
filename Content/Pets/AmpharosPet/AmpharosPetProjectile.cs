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
		public override int hitboxWidth => 80;
		public override int hitboxHeight => 65;

		public override int totalFrames => 30;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,10];
		public override int[] walkStartEnd => [24,29];
		public override int[] jumpStartEnd => [10,15];
		public override int[] fallStartEnd => [12,15];
        public override int[] attackStartEnd => [15, 23];
    }

	public class AmpharosPetProjectileShiny : AmpharosPetProjectile { }
}
