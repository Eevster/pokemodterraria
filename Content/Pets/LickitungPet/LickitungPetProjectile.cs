using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LickitungPet
{
	public class LickitungPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 36;

		public override int totalFrames => 24;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [7,11];
		public override int[] walkStartEnd => [12,19];
		public override int[] jumpStartEnd => [20,23];
		public override int[] fallStartEnd => [21,22];
        public override int[] attackStartEnd => [0, 6];
    }

	public class LickitungPetProjectileShiny : LickitungPetProjectile{}
}
