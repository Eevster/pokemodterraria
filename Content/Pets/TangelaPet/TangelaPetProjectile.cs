using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TangelaPet
{
	public class TangelaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 12;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 9];
		public override int[] jumpStartEnd => [4, 4];
		public override int[] fallStartEnd => [7, 7];
		public override int[] attackStartEnd => [10, 11];
	}

	public class TangelaPetProjectileShiny : TangelaPetProjectile{}
}
