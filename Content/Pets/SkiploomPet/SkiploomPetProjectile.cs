using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SkiploomPet
{
	public class SkiploomPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 58;
        public override int hitboxHeight => 40;

        public override int totalFrames => 44;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [22, 35];
        public override int[] walkStartEnd => [35, 43];
        public override int[] jumpStartEnd => [8, 14];
        public override int[] fallStartEnd => [8, 14];
        public override int[] attackStartEnd => [0, 22];


    }

	public class SkiploomPetProjectileShiny : SkiploomPetProjectile { }
}
