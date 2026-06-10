using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SlowbroPet
{
	public class SlowbroPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 72;
		public override int hitboxHeight => 64;

		public override int totalFrames => 74;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,57];
		public override int[] walkStartEnd => [65,73];
		public override int[] jumpStartEnd => [52,55];
		public override int[] fallStartEnd => [52,55];
        public override int[] attackStartEnd => [58, 64];

        public override string[] evolutions => ["Slowking"];
        public override string[] itemToEvolve => ["KingsRockItem"];
    }

	public class SlowbroPetProjectileShiny : SlowbroPetProjectile{}
}
