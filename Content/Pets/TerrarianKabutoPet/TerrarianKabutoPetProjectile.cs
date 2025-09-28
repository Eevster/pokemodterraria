using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KabutoPet
{
	public class TerrarianKabutoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 28;

		public override int totalFrames => 12;
		public override int animationSpeed => 7;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 6];
        public override int[] attackStartEnd => [7, 11];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [4, 6];
        public override int[] attackFlyStartEnd => [7, 11];

        public override bool tangible => false;

        public override string[] evolutions => ["TerrarianKabutops"];
		public override int levelToEvolve => 40;
		public override int levelEvolutionsNumber => 1;
	}

	public class TerrarianKabutoPetProjectileShiny : TerrarianKabutoPetProjectile{}
}
