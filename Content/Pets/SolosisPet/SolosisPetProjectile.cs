using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SolosisPet
{
	public class SolosisPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 26;

        public override int totalFrames => 14;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 0];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [10, 13];
        public override int[] fallStartEnd => [12, 13];
        public override int[] attackStartEnd => [0, 5];

        public override string[] evolutions => ["Duosion"];
		public override int levelToEvolve => 32;
		public override int levelEvolutionsNumber => 1;

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(0,0);
	}

	public class SolosisPetProjectileShiny : SolosisPetProjectile { }
}
