using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.GravelerPet
{
	public class GravelerPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 48;
		public override int hitboxHeight => 48;

		public override int totalFrames => 12;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [4,7];
		public override int[] jumpStartEnd => [4,4];
		public override int[] fallStartEnd => [7,7];
		public override int[] attackStartEnd => [8,11];

		public override string[] evolutions => ["Golem"];
		public override string[] itemToEvolve => ["LinkingCordItem"];
	}

	public class GravelerPetProjectileShiny : GravelerPetProjectile{}
}
