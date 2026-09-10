using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.OrthwormPet
{
	public class OrthwormPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 88;
		public override int hitboxHeight => 16;

		public override int totalFrames => 1;
		public override int animationSpeed => 15;
		public override int[] idleStartEnd => [0, 0];
		public override int[] walkStartEnd => [0, 0];
		public override int[] jumpStartEnd => [0, 0];
		public override int[] fallStartEnd => [0, 0];
		public override int[] attackStartEnd => [0, 0];

		public override bool canBeMounted => true;
        public override Vector2 playerMountPosition => new Vector2(8,0);
	}

	public class OrthwormPetProjectileShiny : OrthwormPetProjectile{}
}
