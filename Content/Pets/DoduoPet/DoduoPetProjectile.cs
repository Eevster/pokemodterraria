using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DoduoPet
{
	public class DoduoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 4;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];

		public override string[] evolutions => ["Dodrio"];
		public override int levelToEvolve => 31;
		public override int levelEvolutionsNumber => 1;
	}

	public class DoduoPetProjectileShiny : DoduoPetProjectile{}
}
