using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.WigglytuffPet
{
	public class WigglytuffPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 46;

        public override int totalFrames => 22;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 7];
        public override int[] walkStartEnd => [8, 13];
        public override int[] jumpStartEnd => [11, 11];
        public override int[] fallStartEnd => [16, 16];
        public override int[] attackStartEnd => [14, 21];
    }

	public class WigglytuffPetProjectileShiny : WigglytuffPetProjectile{}
}
