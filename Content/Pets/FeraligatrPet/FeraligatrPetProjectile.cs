using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.FeraligatrPet
{
	public class FeraligatrPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 18;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [12,17];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [6,11];
		public override int[] attackSwimStartEnd => [12,17];
	}

	public class FeraligatrPetProjectileShiny : FeraligatrPetProjectile{}
}
