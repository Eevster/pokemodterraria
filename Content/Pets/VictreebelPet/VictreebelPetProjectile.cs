using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VictreebelPet
{
	public class VictreebelPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 34;
        public override int hitboxHeight => 46;

        public override int totalFrames => 8;
		public override int animationSpeed => 7;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [2,2];
		public override int[] fallStartEnd => [3,3];
		public override int[] attackStartEnd => [4,7];
	}

	public class VictreebelPetProjectileShiny : VictreebelPetProjectile{}
}
