using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.VespiquenPet
{
	public class VespiquenPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 46;
        public override int hitboxHeight => 66;

        public override int totalFrames => 18;
        public override int animationSpeed => 8;
        public override int moveStyle => 1;

        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [0, 6];


        public override int[] idleFlyStartEnd => [0, 6];
        public override int[] walkFlyStartEnd => [0, 6];
        public override int[] attackFlyStartEnd => [7, 17];


    }

	public class VespiquenPetProjectileShiny : VespiquenPetProjectile{}
}
