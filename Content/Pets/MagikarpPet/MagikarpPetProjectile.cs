using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MagikarpPet
{
	public class MagikarpPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
		public override int hitboxHeight => 38;

		public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [1,3];
		public override int[] walkStartEnd => [0,4];
		public override int[] jumpStartEnd => [3,4];
		public override int[] fallStartEnd => [5,7];
		public override int[] attackStartEnd => [3, 7];

		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [6, 6];
		public override int[] walkSwimStartEnd => [5, 7];
		public override int[] attackSwimStartEnd => [3, 7];

		public override string[] evolutions => ["Gyarados"];
		public override int levelToEvolve => 21;
		public override int levelEvolutionsNumber => 1;
	}

	public class MagikarpPetProjectileShiny : MagikarpPetProjectile{}
}
