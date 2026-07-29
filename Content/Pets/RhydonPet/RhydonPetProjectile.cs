using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RhydonPet
{
	public class RhydonPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 70;

		public override int totalFrames => 18;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] jumpStartEnd => [10, 11];
        public override int[] fallStartEnd => [12, 12];
        public override int[] attackStartEnd => [13, 17];
	}

	public class RhydonPetProjectileShiny : RhydonPetProjectile{}
}
