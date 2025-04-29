using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.JoltikPet
{
	public class JoltikPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 14;
		public override int hitboxHeight => 10;

		public override int moveStyle => 3;

		public override int totalFrames => 2;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,1];
		public override int[] jumpStartEnd => [1,1];
		public override int[] fallStartEnd => [1,1];
		public override int[] attackStartEnd => [0,0];
		public override int maxJumpHeight => 15;
		public override float fallSpeed => 15f;
		public override float fallAccel => 0.5f;

		public override string[] evolutions => ["Galvantula"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;
	}

	public class JoltikPetProjectileShiny : JoltikPetProjectile{}
}
