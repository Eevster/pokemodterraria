using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TinkatonPet
{
	public class TinkatonPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 36;
        public override int hitboxHeight => 56;

        public override int totalFrames => 28;
        public override int animationSpeed => 8;
        public override int[] idleStartEnd => [0, 3];
        public override int[] walkStartEnd => [4, 7];
        public override int[] jumpStartEnd => [7, 8];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [9, 27];

    }

	public class TinkatonPetProjectileShiny : TinkatonPetProjectile { }
}
