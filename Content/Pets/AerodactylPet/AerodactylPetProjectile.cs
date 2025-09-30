using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AerodactylPet
{
	public class AerodactylPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 60;
		public override int hitboxHeight => 64;

		public override int totalFrames => 15;
		public override int animationSpeed => 6;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] attackStartEnd => [10, 14];

        public override int[] idleFlyStartEnd => [0, 4];
        public override int[] walkFlyStartEnd => [5, 9];
        public override int[] attackFlyStartEnd => [10, 14];
    }

	public class AerodactylPetProjectileShiny : AerodactylPetProjectile{}
}
