using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ScolipedePetMega
{
	public class ScolipedeMegaPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 152;
        public override int hitboxHeight => 120;

        public override int totalFrames => 23;
		public override int animationSpeed => 7;

		

		public override int[] idleStartEnd => [6,16];
		public override int[] walkStartEnd => [17,22];
		public override int[] jumpStartEnd => [17,22];
		public override int[] fallStartEnd => [21,22];
		public override int[] attackStartEnd => [0,6];

		

        public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Scolipede"];
		public override string[] itemToMegaEvolve => ["ScolipedeMegaStoneItem"];

	
	}
	public class ScolipedeMegaPetProjectileShiny : ScolipedeMegaPetProjectile { }
}