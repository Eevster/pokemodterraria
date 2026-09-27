using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.MewtwoPet
{
	public class MewtwoPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 44;
        public override int hitboxHeight => 62;

        public override int totalFrames => 39;
        public override int animationSpeed => 7;
        public override int moveStyle => 2;

        public override int[] idleStartEnd => [9, 16];
        public override int[] walkStartEnd => [33, 38];
        public override int[] jumpStartEnd => [27, 32];
        public override int[] fallStartEnd => [27, 32];
        public override int[] attackStartEnd => [0, 8];

        public override int[] idleFlyStartEnd => [27, 32];
        public override int[] walkFlyStartEnd => [33, 38];
        public override int[] attackFlyStartEnd => [17, 26];
    }

	public class MewtwoPetProjectileShiny : MewtwoPetProjectile{}
}
