using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MachopPet
{
	public class MachopPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

		public override int totalFrames => 7;
		public override int animationSpeed => 12;
		public override int[] idleStartEnd => [0,1];
		public override int[] walkStartEnd => [2,5];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [4,4];
		public override int[] attackStartEnd => [6, 6];

		public override string[] evolutions => ["Machoke"];
		public override int levelToEvolve => 28;
		public override int levelEvolutionsNumber => 1;
	}

	public class MachopPetProjectileShiny : MachopPetProjectile{}
}
