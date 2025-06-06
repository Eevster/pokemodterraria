using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GyaradosPet
{
	public class GyaradosMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 118;
		public override int hitboxHeight => 100;

		public override int totalFrames => 9;
		public override int animationSpeed => 6;
		public override int moveStyle => 1;

		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] attackStartEnd => [4, 8];

		public override int[] idleFlyStartEnd => [0, 3];
		public override int[] walkFlyStartEnd => [0, 3];
		public override int[] attackFlyStartEnd => [4, 8];

		public override bool isMega => true;

		public override string[] megaEvolutionBase => ["Gyarados"];
		public override string[] itemToMegaEvolve => ["GyaradosMegaStoneItem"];
	}

	public class GyaradosMegaPetProjectileShiny : GyaradosMegaPetProjectile{}
}
