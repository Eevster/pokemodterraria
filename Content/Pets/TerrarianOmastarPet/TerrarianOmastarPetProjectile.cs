using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.OmastarPet
{
	public class TerrarianOmastarPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 46;

        public override int totalFrames => 18;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [6, 6];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [9, 17];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [0, 5];
        public override int[] walkSwimStartEnd => [6, 9];
        public override int[] attackSwimStartEnd => [9, 17];
    }

	public class TerrarianOmastarPetProjectileShiny : TerrarianOmastarPetProjectile { }
}
