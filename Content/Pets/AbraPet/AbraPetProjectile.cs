using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AbraPet
{
	public class AbraPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 64;
		public override int hitboxHeight => 64;

        public override int totalFrames => 7;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [0, 3];
        public override int[] jumpStartEnd => [0, 3];
        public override int[] fallStartEnd => [0, 3];
        public override int[] attackStartEnd => [4, 6];

        public override string[] evolutions => ["Kadabra"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
    }

	public class AbraPetProjectileShiny : AbraPetProjectile{}
}
