using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SeelPet
{
	public class SentretPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 16;
        public override int hitboxHeight => 16;

        public override int totalFrames => 32;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [6, 19];
        public override int[] walkStartEnd => [25, 31];
        public override int[] jumpStartEnd => [19, 24];
        public override int[] fallStartEnd => [20, 21];
        public override int[] attackStartEnd => [0, 5];

        public override string[] evolutions => ["Furret"];
		public override int levelToEvolve => 15;
		public override int levelEvolutionsNumber => 1;
	}

	public class SentretPetProjectileShiny : SentretPetProjectile{}
}
