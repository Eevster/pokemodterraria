using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WartortlePet
{
	public class WartortlePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 32;
		public override int hitboxHeight => 36;

		public override int totalFrames => 17;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [8,15];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [11,11];
		public override int[] attackStartEnd => [16,16];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,7];
		public override int[] walkSwimStartEnd => [8,15];
		public override int[] attackSwimStartEnd => [16,16];

		public override float moveSpeed1 => 4f;
		public override float moveSpeed2 => 7f;

		public override string[] evolutions => ["Blastoise"];
		public override int levelToEvolve => 36;
		public override int levelEvolutionsNumber => 1;
	}

	public class WartortlePetProjectileShiny : WartortlePetProjectile{}
}