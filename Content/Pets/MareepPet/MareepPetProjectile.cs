using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MareepPet
{
	public class MareepPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 75;
		public override int hitboxHeight => 60;

        public override int totalFrames => 23;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [6, 15];
        public override int[] jumpStartEnd => [6, 15];
        public override int[] fallStartEnd => [0, 6];
        public override int[] attackStartEnd => [16, 21];

        public override string[] evolutions => ["Flaaffy"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
    }

	public class MareepPetProjectileShiny : MareepPetProjectile{}
}
