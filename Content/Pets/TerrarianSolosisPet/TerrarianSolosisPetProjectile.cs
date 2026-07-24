using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TerrarianSolosisPet
{
	public class TerrarianSolosisPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 34;
        public override int hitboxHeight => 32;



        public override int totalFrames => 25;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 7];
        public override int[] walkStartEnd => [8, 14];
        public override int[] jumpStartEnd => [20, 24];
        public override int[] fallStartEnd => [22, 23];
        public override int[] attackStartEnd => [13, 21];

        public override string[] evolutions => ["TerrarianDuosion"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		
	}

	public class TerrarianSolosisPetProjectileShiny : TerrarianSolosisPetProjectile { }
}
