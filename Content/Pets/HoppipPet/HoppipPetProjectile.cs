using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CleffaPet
{
	public class HoppipPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 20;
        public override int hitboxHeight => 20;

        public override int totalFrames => 61;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [32, 49];
        public override int[] walkStartEnd => [50, 60];
        public override int[] jumpStartEnd => [35, 39];
        public override int[] fallStartEnd => [8, 31];
        public override int[] attackStartEnd => [0, 7];

        public override int[] idleFlyStartEnd => [8,31];
		public override int[] walkFlyStartEnd => [8,31];
		public override int[] attackFlyStartEnd => [0, 7];
    }

	public class HoppipPetProjectileShiny : HoppipPetProjectile { }
}
