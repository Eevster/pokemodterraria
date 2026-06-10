using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.SlowkingPet
{
	public class SlowkingPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 72;
        public override int hitboxHeight => 80;

        public override int totalFrames => 38;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 13];
        public override int[] walkStartEnd => [29, 37];
        public override int[] jumpStartEnd => [14, 20];
        public override int[] fallStartEnd => [17, 19];
        public override int[] attackStartEnd => [20, 28];

    }

	public class SlowkingPetProjectileShiny : SlowkingPetProjectile{}
}
