using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SlowpokePet
{
	public class SlowpokePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 68;
        public override int hitboxHeight => 54;

        public override int totalFrames => 65;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 39];
        public override int[] walkStartEnd => [55, 64];
        public override int[] jumpStartEnd => [39, 48];
        public override int[] fallStartEnd => [39, 48];
        public override int[] attackStartEnd => [47, 55];

        public override string[] evolutions => ["Slowbro", "Slowking"];
		public override int levelToEvolve => 37;
		public override int levelEvolutionsNumber => 1;
        public override string[] itemToEvolve => ["KingsRockItem"];
    }

	public class SlowpokePetProjectileShiny : SlowpokePetProjectile{}
}
