using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CombeePet
{
	public class CombeePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 38;
        public override int hitboxHeight => 30;

        public override int totalFrames => 4;
        public override int animationSpeed => 8;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 1];
        public override int[] walkStartEnd => [0, 1];

        public override int[] idleFlyStartEnd => [0, 1];
        public override int[] walkFlyStartEnd => [0, 1];
        public override int[] attackFlyStartEnd => [2, 3];

        public override string[] evolutions => ["Vespiquen"];
		public override int levelToEvolve => 21;
		public override int levelEvolutionsNumber => 1;

		
	}

	public class CombeePetProjectileShiny : CombeePetProjectile{}
}
