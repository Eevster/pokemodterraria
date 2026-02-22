using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AlolanSandslashPet
{
	public class AlolanSandslashPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 42;

        public override int totalFrames => 20;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 12];
        public override int[] jumpStartEnd => [10, 10];
        public override int[] fallStartEnd => [6, 6];
        public override int[] attackStartEnd => [13, 19];
    }

	public class AlolanSandslashPetProjectileShiny : AlolanSandslashPetProjectile { }
}
