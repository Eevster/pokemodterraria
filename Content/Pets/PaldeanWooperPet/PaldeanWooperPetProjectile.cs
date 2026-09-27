using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WeedlePet
{
	public class PaldeanWooperPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 30;
        public override int hitboxHeight => 30;

        public override int totalFrames => 40;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [13, 17];
        public override int[] walkStartEnd => [0, 6];
        public override int[] jumpStartEnd => [18, 22];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [7, 14];
        public override bool canSwim => true;
        public override int[] idleSwimStartEnd => [30, 35];
        public override int[] walkSwimStartEnd => [36, 39];
        public override int[] attackSwimStartEnd => [24, 29];

        public override string[] evolutions => ["Clodsire"];
		public override int levelToEvolve => 20;
		public override int levelEvolutionsNumber => 1;

        public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(0,0);
	}

	public class PaldeanWooperPetProjectileShiny : PaldeanWooperPetProjectile{}
}
