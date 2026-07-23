using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GrimerPet
{
	public class GrimerPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 28;

		public override int totalFrames => 26;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [11,19];
		public override int[] walkStartEnd => [0,5];
		public override int[] jumpStartEnd => [20,23];
		public override int[] fallStartEnd => [24,24];
		public override int[] attackStartEnd => [9,10];

		public override string[] evolutions => ["Muk"];
		public override int levelToEvolve => 38;
		public override int levelEvolutionsNumber => 1;
	}

	public class GrimerPetProjectileShiny : GrimerPetProjectile{}
}
