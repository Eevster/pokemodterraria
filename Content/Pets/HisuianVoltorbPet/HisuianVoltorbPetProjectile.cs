using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.HisuianVoltorbPet
{
	public class HisuianVoltorbPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 24;
		public override int hitboxHeight => 24;

		public override int totalFrames => 5;
		public override int animationSpeed => 8;
		public override int[] idleStartEnd => [0,0];
		public override int[] walkStartEnd => [0,0];
		public override int[] jumpStartEnd => [0,0];
		public override int[] fallStartEnd => [0,0];
		public override int[] attackStartEnd => [1,4];

        public override bool canRotate => true;

		public override float moveDistance1 => 80f;
		public override float moveDistance2 => 80f;

		public override string[] evolutions => ["HisuianElectrode"];
		public override string[] itemToEvolve => ["LeafStoneItem"];
	}

	public class HisuianVoltorbPetProjectileShiny : HisuianVoltorbPetProjectile{}
}
