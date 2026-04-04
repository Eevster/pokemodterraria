using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WeedlePet
{
	public class WeedlePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 20;

		public override int totalFrames => 15;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,6];
		public override int[] walkStartEnd => [7,13];
		public override int[] jumpStartEnd => [7,9];
		public override int[] fallStartEnd => [9,11];
		public override int[] attackStartEnd => [14,14];

		public override string[] evolutions => ["Kakuna"];
		public override int levelToEvolve => 7;
		public override int levelEvolutionsNumber => 1;
	}

	public class WeedlePetProjectileShiny : WeedlePetProjectile{}
}
