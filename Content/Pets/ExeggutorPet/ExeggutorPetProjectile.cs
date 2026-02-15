using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ExeggutorPet
{
	public class ExeggutorPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 62;

		public override int totalFrames => 18;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 5];
		public override int[] walkStartEnd => [6, 16];
		public override int[] jumpStartEnd => [9, 9];
		public override int[] fallStartEnd => [13, 13];
		public override int[] attackStartEnd => [17, 17];
		

	}

	public class ExeggutorPetProjectileShiny : ExeggutorPetProjectile{}
}
