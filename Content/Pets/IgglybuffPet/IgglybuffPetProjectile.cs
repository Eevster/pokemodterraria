using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CleffaPet
{
	public class IgglybuffPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 16;
        public override int hitboxHeight => 22;

        public override int totalFrames => 20;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 8];
        public override int[] walkStartEnd => [9, 16];
        public override int[] jumpStartEnd => [9, 10];
        public override int[] fallStartEnd => [11, 12];
        public override int[] attackStartEnd => [16, 19];

        public override string[] evolutions => ["Jigglypuff"];
        public override string[] specialConditionToEvolve => ["Happiness"];
    }

	public class IgglybuffPetProjectileShiny : IgglybuffPetProjectile { }
}
