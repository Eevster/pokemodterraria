using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WhirlipedePet
{
	public class TerrarianDuosionPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 70;
        public override int hitboxHeight => 48;



        public override int totalFrames => 22;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [11, 21];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [4, 5];
        public override int[] fallStartEnd => [5, 5];
        public override int[] attackStartEnd => [0, 3];

        public override string[] evolutions => ["TerrarianReuniclus"];
		public override int levelToEvolve => 41;
		public override int levelEvolutionsNumber => 1;

		
	}

	public class TerrarianDuosionPetProjectileShiny : TerrarianDuosionPetProjectile { }
}
