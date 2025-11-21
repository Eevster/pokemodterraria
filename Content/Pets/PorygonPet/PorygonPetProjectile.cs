using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PorygonPet
{
	public class PorygonPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 30;

		public override int totalFrames => 8;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0,2];
		public override int[] walkStartEnd => [3,6];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [2, 2];
		public override int[] attackStartEnd => [7,7];
	}

	public class PorygonPetProjectileShiny : PorygonPetProjectile{}
}
