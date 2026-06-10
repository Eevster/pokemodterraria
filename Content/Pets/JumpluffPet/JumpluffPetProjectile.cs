using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.JumpluffPet
{
	public class JumpluffPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 64;
        public override int hitboxHeight => 60;

        public override int totalFrames => 59;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [20, 29];
        public override int[] walkStartEnd => [41, 58];
        public override int[] jumpStartEnd => [29, 34];
        public override int[] fallStartEnd => [10, 19];
        public override int[] attackStartEnd => [34, 41];

        public override int[] idleFlyStartEnd => [10,19];
		public override int[] walkFlyStartEnd => [10,19];
		public override int[] attackFlyStartEnd => [0, 10];
    }

	public class JumpluffPetProjectileShiny : JumpluffPetProjectile { }
}
