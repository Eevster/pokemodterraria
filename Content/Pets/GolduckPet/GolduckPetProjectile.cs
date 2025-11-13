using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GolduckPet
{
	public class GolduckPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 18;
		public override int animationSpeed =>10;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [6, 6];
		public override int[] attackStartEnd => [8, 8];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [9,12];
		public override int[] walkSwimStartEnd => [13,16];
		public override int[] attackSwimStartEnd => [17,17];
	}

	public class GolduckPetProjectileShiny : GolduckPetProjectile{}
}
