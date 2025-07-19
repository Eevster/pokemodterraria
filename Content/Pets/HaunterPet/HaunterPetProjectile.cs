using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.HaunterPet
{
	public class HaunterPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 34;
        public override int hitboxHeight => 40;

        public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];

        public override string[] evolutions => ["Gengar"];
        public override string[] itemToEvolve => ["LinkingCordItem"];

        public override bool tangible => false;

		public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.light = 0.2f;
        }
    }

	public class HaunterPetProjectileShiny : HaunterPetProjectile{}
}
