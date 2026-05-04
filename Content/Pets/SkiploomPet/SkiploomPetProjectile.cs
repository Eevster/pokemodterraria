using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SkiploomPet
{
	public class SkiploomPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 30;
        public override int hitboxHeight => 26;

        public override int totalFrames => 44;
        public override int animationSpeed => 5;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [22, 35];
        public override int[] walkStartEnd => [36, 43];
        public override int[] jumpStartEnd => [8, 11];
        public override int[] fallStartEnd => [12, 13];
        public override int[] attackStartEnd => [14, 21];

        public override int[] idleFlyStartEnd => [8,13];
		public override int[] walkFlyStartEnd => [8,13];
		public override int[] attackFlyStartEnd => [0, 7];
    }

	public class SkiploomPetProjectileShiny : SkiploomPetProjectile { }
}
