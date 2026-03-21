using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DoduoPet
{
	public class FlaaffyPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 58;
        public override int hitboxHeight => 55;

        public override int totalFrames => 36;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 9];
        public override int[] walkStartEnd => [26, 35];
        public override int[] jumpStartEnd => [9, 14];
        public override int[] fallStartEnd => [11, 13];
        public override int[] attackStartEnd => [14, 26];

        public override string[] evolutions => ["Ampharos"];
		public override int levelToEvolve => 30;
		public override int levelEvolutionsNumber => 1;
	}

	public class FlaaffyPetProjectileShiny : FlaaffyPetProjectile { }
}
