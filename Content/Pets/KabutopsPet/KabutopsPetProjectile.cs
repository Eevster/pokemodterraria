using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.KabutopsPet
{
	public class KabutopsPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 28;
		public override int hitboxHeight => 46;

        public override int totalFrames => 26;
        public override int animationSpeed => 9;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] jumpStartEnd => [18, 18];
        public override int[] fallStartEnd => [16, 16];
        public override int[] attackStartEnd => [8, 14];

        public override bool canSwim => true;

        public override int[] idleSwimStartEnd => [15, 18];
        public override int[] walkSwimStartEnd => [15, 18];
        public override int[] attackSwimStartEnd => [19, 25];
    }

	public class KabutopsPetProjectileShiny : KabutopsPetProjectile{}
}
