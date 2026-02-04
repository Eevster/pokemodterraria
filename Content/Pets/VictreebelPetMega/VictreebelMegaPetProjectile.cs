using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VictreebelMegaPetMega
{
	public class VictreebelMegaPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 68;
        public override int hitboxHeight => 88;

        public override int totalFrames => 16;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [6,6];
		public override int[] fallStartEnd => [4,4];
		public override int[] attackStartEnd => [8,15];

		public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Victreebel"];
		public override string[] itemToMegaEvolve => ["VictreebelMegaStoneItem"];
	}

	public class VictreebelMegaPetProjectileShiny : VictreebelMegaPetProjectile{}
}
