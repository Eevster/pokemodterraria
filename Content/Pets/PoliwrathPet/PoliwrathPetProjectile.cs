using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PoliwrathPet
{
	public class PoliwrathPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 34;
        public override int hitboxHeight => 46;

        public override int totalFrames => 16;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [6,6];
		public override int[] attackStartEnd => [8,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [12,15];
		public override int[] walkSwimStartEnd => [12,15];
		public override int[] attackSwimStartEnd => [8,11];
	}

	public class PoliwrathPetProjectileShiny : PoliwrathPetProjectile{}
}
