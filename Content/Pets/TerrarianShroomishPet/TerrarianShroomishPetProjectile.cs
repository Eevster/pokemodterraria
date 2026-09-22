using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TerrarianShroomishPet
{
	public class TerrarianShroomishPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 30;

        public override int totalFrames => 22;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [5, 13];
        public override int[] walkStartEnd => [16, 21];
        public override int[] jumpStartEnd => [13, 15];
        public override int[] fallStartEnd => [14, 15];
        public override int[] attackStartEnd => [0, 5];

        public override string[] evolutions => ["TerrarianBreloom"];
		public override int levelToEvolve => 23;
		public override int levelEvolutionsNumber => 1;
	}

	public class TerrarianShroomishPetProjectileShiny : TerrarianShroomishPetProjectile{}
}
