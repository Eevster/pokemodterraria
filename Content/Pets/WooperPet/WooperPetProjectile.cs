using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WeedlePet
{
	public class WooperPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 18;
        public override int hitboxHeight => 26;

        public override int totalFrames => 32;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 11];
        public override int[] walkStartEnd => [24, 31];
        public override int[] jumpStartEnd => [13, 18];
        public override int[] fallStartEnd => [16, 16];
        public override int[] attackStartEnd => [17, 23];

        public override string[] evolutions => ["Quagsire"];
		public override int levelToEvolve => 20;
		public override int levelEvolutionsNumber => 1;

        public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(0,0);
	}

	public class WooperPetProjectileShiny : WooperPetProjectile{}
}
