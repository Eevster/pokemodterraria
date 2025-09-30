using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KabutopsPet
{
	public class TerrarianKabutopsPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

        public override int totalFrames => 15;
        public override int animationSpeed => 9;
        public override int moveStyle => 1;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] attackStartEnd => [8, 14];

        public override int[] idleFlyStartEnd => [0, 3];
        public override int[] walkFlyStartEnd => [4, 7];
        public override int[] attackFlyStartEnd => [8, 14];
        public override bool tangible => false;
    }

	public class TerrarianKabutopsPetProjectileShiny : TerrarianKabutopsPetProjectile{}
}
