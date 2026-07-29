using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LickilickyPet
{
	public class LickilickyPetProjectile : PokemonPetProjectile
	{
		public override int hitboxWidth => 40;
        public override int hitboxHeight => 52;

        public override int totalFrames => 31;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [8, 23];
        public override int[] walkStartEnd => [27, 30];
        public override int[] jumpStartEnd => [24, 26];
        public override int[] fallStartEnd => [24, 24];
        public override int[] attackStartEnd => [0, 7];
    }

	public class LickilickyPetProjectileShiny : LickilickyPetProjectile{}
}
