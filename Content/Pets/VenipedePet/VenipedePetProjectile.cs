using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenipedePet
{
	public class VenipedePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 60;
        public override int hitboxHeight => 44;



        public override int totalFrames => 36;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [7, 15];
        public override int[] walkStartEnd => [16, 22];
        public override int[] jumpStartEnd => [0, 6];
        public override int[] fallStartEnd => [2, 4];
        public override int[] attackStartEnd => [0, 6];

        public override string[] evolutions => ["Whirlipede"];
		public override int levelToEvolve => 22;
		public override int levelEvolutionsNumber => 1;

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
	}

	public class VenipedePetProjectileShiny : VenipedePetProjectile{}
}
