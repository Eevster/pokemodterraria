using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MarowakPet
{
	public class MarowakPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 10;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 8];
		public override int[] jumpStartEnd => [5, 5];
		public override int[] fallStartEnd => [8, 8];
		public override int[] attackStartEnd => [9, 9];
	}

	public class MarowakPetProjectileShiny : MarowakPetProjectile{}
}
