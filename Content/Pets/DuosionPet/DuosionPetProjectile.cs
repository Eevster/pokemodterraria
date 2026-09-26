using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DuosionPet
{
	public class DuosionPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 34;

        public override int totalFrames => 20;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [16, 19];
        public override int[] walkStartEnd => [8, 11];
        public override int[] jumpStartEnd => [12, 15];
        public override int[] fallStartEnd => [15, 16];
        public override int[] attackStartEnd => [0, 7];

        public override string[] evolutions => ["Reuniclus"];
		public override int levelToEvolve => 41;
		public override int levelEvolutionsNumber => 1;
	}

	public class DuosionPetProjectileShiny : DuosionPetProjectile { }
}
