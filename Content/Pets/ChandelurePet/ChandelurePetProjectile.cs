using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LampentPet
{
	public class ChandelurePetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 90;
        public override int hitboxHeight => 44;

        public override int moveStyle => 1;

        public override int totalFrames => 21;
        public override int animationSpeed => 11;
        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [17, 20];
        public override int[] idleFlyStartEnd => [0, 6];
        public override int[] walkFlyStartEnd => [17, 20];
        public override int[] attackFlyStartEnd => [7, 15];


    }

	public class ChandelurePetProjectileShiny : ChandelurePetProjectile{}
}
