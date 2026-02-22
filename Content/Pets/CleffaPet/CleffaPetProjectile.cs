using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CleffaPet
{
	public class CleffaPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 16;
        public override int hitboxHeight => 16;

        public override int totalFrames => 10;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [6, 8];
        public override int[] jumpStartEnd => [6, 6];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [9, 9];

        public override string[] evolutions => ["Clefairy"];
        public override string[] specialConditionToEvolve => ["Happiness"];
    }

	public class CleffaPetProjectileShiny : CleffaPetProjectile{}
}
