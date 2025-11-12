using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MrMimePet
{
	public class MrMimePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 13;

		public override int animationSpeed => 12;

		public override int[] idleStartEnd => [0, 5];

		public override int[] walkStartEnd => [6, 11];

		public override int[] jumpStartEnd => [6, 6];

		public override int[] fallStartEnd => [9, 9];

		public override int[] attackStartEnd => [12, 12];
	}

	public class MrMimePetProjectileShiny : MrMimePetProjectile{}
}
