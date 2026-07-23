using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AmpharosPetMega
{
	public class AmpharosMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 56;

		public override int totalFrames => 32;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,7];
		public override int[] walkStartEnd => [24,31];
		public override int[] jumpStartEnd => [8,13];
		public override int[] fallStartEnd => [14,15];
        public override int[] attackStartEnd => [16, 23];

		public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Ampharos"];
		public override string[] itemToMegaEvolve => ["AmpharosMegaStoneItem"];
    }

	public class AmpharosMegaPetProjectileShiny : AmpharosMegaPetProjectile { }
}
