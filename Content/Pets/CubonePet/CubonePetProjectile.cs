using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CubonePet
{
	public class CubonePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 11;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,9];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [10, 10];

		public override string[] evolutions => ["Marowak"];
		public override int levelToEvolve => 28;
		public override int levelEvolutionsNumber => 1;
	}

	public class CubonePetProjectileShiny : CubonePetProjectile{}
}
