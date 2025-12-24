using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KrabbyPet
{
	public class KrabbyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 30;

		public override int totalFrames => 11;
		public override int animationSpeed => 10;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,9];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [10, 10];

		public override string[] evolutions => ["Kingler"];
		public override int levelToEvolve => 28;
		public override int levelEvolutionsNumber => 1;
	}

	public class KrabbyPetProjectileShiny : KrabbyPetProjectile{}
}
