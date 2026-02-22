using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ClefairyPet
{
	public class ClefairyPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 28;

        public override int totalFrames => 16;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 7];
        public override int[] walkStartEnd => [8, 11];
        public override int[] jumpStartEnd => [10, 10];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [12, 15];

        public override string[] evolutions => ["Clefable"];
		public override int levelToEvolve => 20;
		public override int levelEvolutionsNumber => 1;
	}

	public class ClefairyPetProjectileShiny : ClefairyPetProjectile{}
}
