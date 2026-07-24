using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.TerrarianReuniclusPet
{
	public class TerrarianReuniclusPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 94;
        public override int hitboxHeight => 84;



        public override int totalFrames => 26;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 10];
        public override int[] walkStartEnd => [10, 14];
        public override int[] jumpStartEnd => [21, 25];
        public override int[] fallStartEnd => [24, 24];
        public override int[] attackStartEnd => [15, 21];






    }

	public class TerrarianReuniclusPetProjectileShiny : TerrarianReuniclusPetProjectile { }
}
