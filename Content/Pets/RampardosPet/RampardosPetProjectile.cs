using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.RampardosPet
{
	public class RampardosPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 13;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 9];
        public override int[] jumpStartEnd => [8, 8];
        public override int[] fallStartEnd => [6, 6];
        public override int[] attackStartEnd => [10, 12];

    }

	public class RampardosPetProjectileShiny : RampardosPetProjectile{}
}
