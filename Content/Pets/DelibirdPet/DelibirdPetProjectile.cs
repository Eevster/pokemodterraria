using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.DelibirdPet
{
	public class DelibirdPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 46;

        public override int totalFrames => 57;

        public override int animationSpeed => 5;

        public override int[] idleStartEnd => [15, 30];

        public override int[] walkStartEnd => [46, 56];

        public override int[] jumpStartEnd => [31, 37];

        public override int[] fallStartEnd => [8, 14];

        public override int[] attackStartEnd => [37, 46];
    }

	public class DelibirdPetProjectileShiny : DelibirdPetProjectile{}
}
