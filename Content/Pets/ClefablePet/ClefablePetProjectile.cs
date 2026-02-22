using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ClefablePet
{
	public class ClefablePetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

        public override int totalFrames => 14;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 8];
        public override int[] jumpStartEnd => [7, 7];
        public override int[] fallStartEnd => [5, 5];
        public override int[] attackStartEnd => [9, 13];
    }

	public class ClefablePetProjectileShiny : ClefablePetProjectile{}
}
