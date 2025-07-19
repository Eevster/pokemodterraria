using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.NidorinoPet
{
	public class NidorinoPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 30;

        public override int totalFrames => 16;
		public override int animationSpeed => 6;
		public override int[] idleStartEnd => [0,4];
		public override int[] walkStartEnd => [5,9];
		public override int[] jumpStartEnd => [7,7];
		public override int[] fallStartEnd => [9,9];
		public override int[] attackStartEnd => [10,15];

        public override string[] evolutions => ["Nidoking"];
        public override string[] itemToEvolve => ["MoonStoneItem"];
	}

	public class NidorinoPetProjectileShiny : NidorinoPetProjectile{}
}
