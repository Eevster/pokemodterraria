using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SudowoodoPet
{
	public class SudowoodoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 48;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0, 15];
		public override int[] walkStartEnd => [16, 21];
		public override int[] jumpStartEnd => [16, 16];
		public override int[] fallStartEnd => [17, 17];
	}

	public class SudowoodoPetProjectileShiny : SudowoodoPetProjectile{}
}
