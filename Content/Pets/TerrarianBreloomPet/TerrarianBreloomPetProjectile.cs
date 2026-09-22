using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TerrarianBreloomPet
{
	public class TerrarianBreloomPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 40;
        public override int hitboxHeight => 34;

        public override int totalFrames => 20;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [6, 9];
        public override int[] walkStartEnd => [14, 19];
        public override int[] jumpStartEnd => [10, 13];
        public override int[] fallStartEnd => [12, 13];
        public override int[] attackStartEnd => [0, 6];
    }

	public class TerrarianBreloomPetProjectileShiny : TerrarianBreloomPetProjectile{ }
}
