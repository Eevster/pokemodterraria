using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenonatPet
{
	public class VenonatPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 40;
        public override int hitboxHeight => 58;

        public override int totalFrames => 15;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 7];
        public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [9, 14];

        public override string[] evolutions => ["Venomoth"];
		public override int levelToEvolve => 31;
		public override int levelEvolutionsNumber => 1;
	}

	public class VenonatPetProjectileShiny : VenonatPetProjectile{}
}
