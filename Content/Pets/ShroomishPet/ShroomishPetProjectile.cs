using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ShroomishPet
{
	public class ShroomishPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 28;

        public override int totalFrames => 28;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [7, 19];
        public override int[] walkStartEnd => [21, 27];
        public override int[] jumpStartEnd => [19, 21];
        public override int[] fallStartEnd => [20, 20];
        public override int[] attackStartEnd => [0, 7];

        public override string[] evolutions => ["Breloom"];
		public override int levelToEvolve => 23;
		public override int levelEvolutionsNumber => 1;
	}

	public class ShroomishPetProjectileShiny : ShroomishPetProjectile{}
}
