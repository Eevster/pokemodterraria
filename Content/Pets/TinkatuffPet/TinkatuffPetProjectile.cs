using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TinkatuffPet
{
	public class TinkatuffPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 62;
        public override int hitboxHeight => 58;

        public override int totalFrames => 40;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] jumpStartEnd => [10, 13];
        public override int[] fallStartEnd => [11, 12];
        public override int[] attackStartEnd => [14, 39];

        public override string[] evolutions => ["Tinkaton"];
		public override int levelToEvolve => 38;
		public override int levelEvolutionsNumber => 1;
	}

	public class TinkatuffPetProjectileShiny : TinkatuffPetProjectile { }
}
