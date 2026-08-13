using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MimeJrPet
	
{
	public class MimeJrPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 16;
		public override int hitboxHeight => 16;

        public override int totalFrames => 10;
        public override int animationSpeed => 10;
        public override int[] idleStartEnd => [0, 2];
        public override int[] walkStartEnd => [3, 7];
        public override int[] jumpStartEnd => [4, 4];
        public override int[] fallStartEnd => [2, 2];
        public override int[] attackStartEnd => [8, 9];

        public override string[] evolutions => ["MrMime"];
		public override int levelToEvolve => 15;
		public override int levelEvolutionsNumber => 1;

        public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-1,0);
		
	}

	public class MimeJrPetProjectileShiny : MimeJrPetProjectile{}
}