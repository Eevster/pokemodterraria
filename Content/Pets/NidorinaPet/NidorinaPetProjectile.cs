using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.NidorinaPet
{
	public class NidorinaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 48;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [9,9];
		public override int[] fallStartEnd => [6,6];
        public override int[] attackStartEnd => [8,11];

        public override string[] evolutions => ["Nidoqueen"];
        public override string[] itemToEvolve => ["MoonStoneItem"];
    }

	public class NidorinaPetProjectileShiny : NidorinaPetProjectile{}
}
