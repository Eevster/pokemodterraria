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
		public override int hitboxHeight => 46;

		public override int totalFrames => 17;
		public override int animationSpeed => 3;
		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [6,11];
		public override int[] jumpStartEnd => [8,8];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [12, 16];

		public override string[] evolutions => ["Golduck"];
		public override int levelToEvolve => 33;
		public override int levelEvolutionsNumber => 1;
	}

	public class PsyduckPetProjectileShiny : PsyduckPetProjectile{}
}
