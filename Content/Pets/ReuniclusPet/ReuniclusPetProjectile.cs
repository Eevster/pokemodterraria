using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ReuniclusPet
{
	public class ReuniclusPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 36;
        public override int hitboxHeight => 40;

		public override int totalFrames => 15;
		public override int animationSpeed => 6;

        public override int moveStyle => 1;
        public override int[] idleStartEnd => [5, 9];
        public override int[] walkStartEnd => [10, 14];
        public override int[] idleFlyStartEnd => [5, 9];
        public override int[] walkFlyStartEnd => [10, 14];
        public override int[] attackFlyStartEnd => [0, 4];
    }

	public class ReuniclusPetProjectileShiny : ReuniclusPetProjectile { }
}
