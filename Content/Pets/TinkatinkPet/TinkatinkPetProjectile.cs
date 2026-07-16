using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TinkatinkPet
{
	public class TinkatinkPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 20;
        public override int hitboxHeight => 24;

        public override int totalFrames => 18;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [8, 10];
        public override int[] jumpStartEnd => [10, 12];
        public override int[] fallStartEnd => [11, 11];
        public override int[] attackStartEnd => [12, 17];

        public override string[] evolutions => ["Tinkatuff"];
		public override int levelToEvolve => 24;
		public override int levelEvolutionsNumber => 1;
    }

	public class TinkatinkPetProjectileShiny : TinkatinkPetProjectile{}
}
