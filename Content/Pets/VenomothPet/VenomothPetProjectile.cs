using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VenomothPet
{
	public class VenomothPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 112;
		public override int hitboxHeight => 88;
        public override int moveStyle => 2;

        public override int totalFrames => 8;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [0,3];
		public override int[] walkStartEnd => [0,3];
		public override int[] jumpStartEnd => [0,3];
		public override int[] fallStartEnd => [0,3];
        public override int[] attackStartEnd => [4, 7];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [0, 3];
        public override int[] attackFlyStartEnd => [4, 7];
    }

	public class VenomothPetProjectileShiny : VenomothPetProjectile{}
}
