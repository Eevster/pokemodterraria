using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DelibirdPet
{
	public class DelibirdPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 57;

		public override int animationSpeed => 5;

		public override int[] idleStartEnd => [0, 16];

		public override int[] walkStartEnd => [17, 26];

		public override int[] jumpStartEnd => [27, 32];

		public override int[] fallStartEnd => [33, 40];

		public override int[] attackStartEnd => [41, 50];
	}

	public class DelibirdPetProjectileShiny : DelibirdPetProjectile{}
}
