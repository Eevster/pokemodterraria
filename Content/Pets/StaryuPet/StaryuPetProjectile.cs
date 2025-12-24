using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.StaryuPet
{
	public class StaryuPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 22;
		public override int hitboxHeight => 22;

		public override int totalFrames => 13;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [1,4];
		public override int[] jumpStartEnd => [3,3];
		public override int[] fallStartEnd => [1,1];
        public override int[] attackStartEnd => [5,8];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [9,12];
        public override int[] walkSwimStartEnd => [9,12];
		public override int[] attackSwimStartEnd => [5,8];

        public override string[] evolutions => ["Starmie"];
        public override string[] itemToEvolve => ["WaterStoneItem"];
	}

	public class StaryuPetProjectileShiny : StaryuPetProjectile{}
}
