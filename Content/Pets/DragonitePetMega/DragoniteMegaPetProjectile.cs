using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using Pokemod.Content.Projectiles;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DragonitePetMega
{
	public class DragoniteMegaPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 48;
		public override int hitboxHeight => 70;

		public override int totalFrames => 12;
		public override int animationSpeed => 6;
		public override int moveStyle => 2;

		public override int[] idleStartEnd => [0,5];
		public override int[] walkStartEnd => [0,5];
		public override int[] attackStartEnd => [6,11];

		public override int[] idleFlyStartEnd => [0,5];
		public override int[] walkFlyStartEnd => [0,5];
		public override int[] attackFlyStartEnd => [6,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [0,5];
		public override int[] walkSwimStartEnd => [0,5];
		public override int[] attackSwimStartEnd => [6,11];

        public override bool isMega => true;
		
		public override string[] megaEvolutionBase => ["Dragonite"];
		public override string[] itemToMegaEvolve => ["DragoniteMegaStoneItem"];
	}
	public class DragoniteMegaPetProjectileShiny : DragoniteMegaPetProjectile{}
}