using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TentacoolPet
{
	public class TentacoolPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

        public override int totalFrames => 22;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 13];
        public override int[] jumpStartEnd => [9, 12];
        public override int[] fallStartEnd => [4, 8];
        public override int[] attackStartEnd => [14, 21];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [0, 5];
        public override int[] walkSwimStartEnd => [6, 13];
        public override int[] attackSwimStartEnd => [14, 21];

        public override string[] evolutions => ["Tentacruel"];
		public override int levelToEvolve => 30;
		public override int levelEvolutionsNumber => 1;
	}

	public class TentacoolPetProjectileShiny : TentacoolPetProjectile{}
}
