using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.PoliwhirlPet
{
	public class PoliwhirlPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 32;

		public override int totalFrames => 16;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [5,5];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8,11];
		
		public override bool canSwim => true;

		public override int[] idleSwimStartEnd => [12,15];
		public override int[] walkSwimStartEnd => [12,15];
		public override int[] attackSwimStartEnd => [8,11];

		public override string[] evolutions => ["Poliwrath"];
		public override string[] itemToEvolve => ["WaterStoneItem"];
	}

	public class PoliwhirlPetProjectileShiny : PoliwhirlPetProjectile{}
}
