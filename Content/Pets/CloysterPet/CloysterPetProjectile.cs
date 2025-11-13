using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CloysterPet
{
	public class CloysterPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 14;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [5, 5];
		public override int[] attackStartEnd => [13, 13];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [0,5];
		public override int[] attackSwimStartEnd => [13, 13];
		
		
	}

	public class CloysterPetProjectileShiny : CloysterPetProjectile{}
}
