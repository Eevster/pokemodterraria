using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KabutoPet
{
	public class KabutoPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 28;

		public override int totalFrames => 16;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0, 3];
		public override int[] walkStartEnd => [4, 7];
		public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [11, 11];
        public override int[] attackStartEnd => [8, 11];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [12, 15];
        public override int[] walkSwimStartEnd => [12, 15];
        public override int[] attackSwimStartEnd => [8, 11];

        public override string[] evolutions => ["Kabutops"];
		public override int levelToEvolve => 40;
		public override int levelEvolutionsNumber => 1;
	}

	public class KabutoPetProjectileShiny : KabutoPetProjectile{}
}
