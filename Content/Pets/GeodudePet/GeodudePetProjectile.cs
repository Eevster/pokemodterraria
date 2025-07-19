using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GeodudePet
{
	public class GeodudePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 38;

        public override int totalFrames => 8;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [4,7];

		public override string[] evolutions => ["Graveler"];
		public override int levelToEvolve => 25;
		public override int levelEvolutionsNumber => 1;
	}

	public class GeodudePetProjectileShiny : GeodudePetProjectile{}
}
