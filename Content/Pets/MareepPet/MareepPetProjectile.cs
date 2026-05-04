using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MareepPet
{
	public class MareepPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 20;
		public override int hitboxHeight => 24;

		public override int totalFrames => 26;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [21,25];
		public override int[] jumpStartEnd => [7,10];
		public override int[] fallStartEnd => [0,2];
        public override int[] attackStartEnd => [11, 20];

        public override string[] evolutions => ["Flaaffy"];
		public override int levelToEvolve => 16;
		public override int levelEvolutionsNumber => 1;
    }

	public class MareepPetProjectileShiny : MareepPetProjectile{}
}
