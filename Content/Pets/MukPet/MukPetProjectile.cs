using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MukPet
{
	public class MukPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 36;

		public override int totalFrames => 22;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [10,16];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [17,20];
		public override int[] fallStartEnd => [21,21];
		public override int[] attackStartEnd => [4,9];
	}

	public class MukPetProjectileShiny : MukPetProjectile{}
}
