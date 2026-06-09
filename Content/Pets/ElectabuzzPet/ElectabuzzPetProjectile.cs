using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.ElectabuzzPet
{
	public class ElectabuzzPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 56;
        public override int hitboxHeight => 56;

        public override int totalFrames => 24;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 5];
        public override int[] walkStartEnd => [5, 12];
        public override int[] jumpStartEnd => [0, 5];
        public override int[] fallStartEnd => [0, 5];
        public override int[] attackStartEnd => [12, 23];
    }

	public class ElectabuzzPetProjectileShiny : ElectabuzzPetProjectile{}
}
