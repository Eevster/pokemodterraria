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

		public override int totalFrames => 15;

		public override int animationSpeed => 7;

		public override int[] idleStartEnd => [0, 7];

		public override int[] walkStartEnd => [8, 13];

		public override int[] jumpStartEnd => [8, 8];

		public override int[] fallStartEnd => [11, 11];

		public override int[] attackStartEnd => [14, 14];
	}

	public class MrMimePetProjectileShiny : MrMimePetProjectile{}
}
