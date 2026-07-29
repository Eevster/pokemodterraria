using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RhyhornPet
{
	public class RhyhornPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 44;
		public override int hitboxHeight => 34;

		public override int totalFrames => 18;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 8];
        public override int[] jumpStartEnd => [9, 10];
        public override int[] fallStartEnd => [11, 13];
        public override int[] attackStartEnd => [14, 17];

		public override string[] evolutions => ["Rhydon"];
		public override int levelToEvolve => 42;
		public override int levelEvolutionsNumber => 1;
	}

	public class RhyhornPetProjectileShiny : RhyhornPetProjectile{}
}
