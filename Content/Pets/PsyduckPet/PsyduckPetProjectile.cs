using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PsyduckPet
{
	public class PsyduckPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 30;

		public override int totalFrames => 24;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,13];
		public override int[] jumpStartEnd => [11,11];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [14, 14];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [15,18];
		public override int[] walkSwimStartEnd => [19,22];
		public override int[] attackSwimStartEnd => [23,23];

		public override string[] evolutions => ["Golduck"];
		public override int levelToEvolve => 33;
		public override int levelEvolutionsNumber => 1;
	}

	public class PsyduckPetProjectileShiny : PsyduckPetProjectile{}
}
