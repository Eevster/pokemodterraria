using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SlowbroPetMega
{
	public class SlowbroMegaPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 36;
		public override int hitboxHeight => 62;

		public override int totalFrames => 28;
		public override int animationSpeed => 5;
		public override int[] idleStartEnd => [9,16];
		public override int[] walkStartEnd => [22,27];
		public override int[] jumpStartEnd => [17,20];
		public override int[] fallStartEnd => [21,21];
        public override int[] attackStartEnd => [0, 8];
    }

	public class SlowbroMegaPetProjectileShiny : SlowbroMegaPetProjectile{}
}
