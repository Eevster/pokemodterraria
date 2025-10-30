using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MachampPet
{
	public class MachampPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 11;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [5, 5];
		public override int[] fallStartEnd => [7, 7];
		public override int[] attackStartEnd => [8, 10];
	}

	public class MachampPetProjectileShiny : MachampPetProjectile{}
}
