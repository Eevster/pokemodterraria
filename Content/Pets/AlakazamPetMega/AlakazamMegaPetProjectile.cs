using System;
using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CharizardPetMegaX
{
	public class AlakazamMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 98;
		public override int hitboxHeight => 94;

		public override int totalFrames => 10;
		public override int animationSpeed => 8;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] attackStartEnd => [4,9];

		public override int[] idleFlyStartEnd => [0, 3];
		public override int[] walkFlyStartEnd => [0, 3];
		public override int[] attackFlyStartEnd => [4, 9];

        public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Alakazam"];
		public override string[] itemToMegaEvolve => ["AlakazamMegaStoneItem"];
	}

	public class AlakazamMegaPetProjectileShiny : AlakazamMegaPetProjectile{ }
}