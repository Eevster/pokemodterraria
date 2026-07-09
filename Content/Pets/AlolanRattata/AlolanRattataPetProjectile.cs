using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AlolanRattataPet
{
	public class AlolanRattataPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 20;

		public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];

		public override string[] evolutions => ["AlolanRaticate"];
		public override string[] specialConditionToEvolve => ["Night"];
		public override int levelToEvolve => 20;
		public override int levelEvolutionsNumber => 1;

		public override bool canBeHeld => true;
        public override Vector2 heldByPlayerPosition => new Vector2(-2,0);
	}

	public class AlolanRattataPetProjectileShiny : AlolanRattataPetProjectile{}
}
