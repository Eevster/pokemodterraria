using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MeganiumPet
{
	public class MeganiumPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 18;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,10];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [10,10];
		public override int[] attackStartEnd => [11,17];
	}

	public class MeganiumPetProjectileShiny : MeganiumPetProjectile{}
}
