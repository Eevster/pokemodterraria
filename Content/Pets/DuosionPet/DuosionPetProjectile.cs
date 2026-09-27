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
        public override int hitboxWidth => 24;
        public override int hitboxHeight => 24;

		public override int moveStyle => 1;
		
		public override int totalFrames => 20;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [11,19];
		public override int[] walkStartEnd => [7,10];
		public override int[] idleFlyStartEnd => [11,19];
        public override int[] walkFlyStartEnd => [7,10];
        public override int[] attackFlyStartEnd => [0,6];

        public override string[] evolutions => ["Reuniclus"];
		public override int levelToEvolve => 41;
		public override int levelEvolutionsNumber => 1;
	}

	public class DuosionPetProjectileShiny : DuosionPetProjectile { }
}
