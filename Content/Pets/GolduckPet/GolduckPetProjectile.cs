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
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 7];
		public override int[] walkStartEnd => [8, 16];
		public override int[] jumpStartEnd => [10, 10];
		public override int[] fallStartEnd => [13, 13];
		public override int[] attackStartEnd => [17, 17];
	}

	public class GolduckPetProjectileShiny : GolduckPetProjectile{}
}
