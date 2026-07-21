using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SeakingPet
{
	public class FurretPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 16;
        public override int hitboxHeight => 16;

        public override int totalFrames => 23;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [6, 12];
        public override int[] walkStartEnd => [19, 22];
        public override int[] jumpStartEnd => [13, 19];
        public override int[] fallStartEnd => [17, 18];
        public override int[] attackStartEnd => [0, 6];
    }

	public class FurretPetProjectileShiny : FurretPetProjectile { }
}
