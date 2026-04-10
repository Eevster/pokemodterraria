using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.CleffaPet
{
	public class HoppipPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 92;
        public override int hitboxHeight => 60;

        public override int totalFrames => 58;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 16];
        public override int[] walkStartEnd => [47, 57];
        public override int[] jumpStartEnd => [0, 0];
        public override int[] fallStartEnd => [17, 39];
        public override int[] attackStartEnd => [40, 47];

        
    }

	public class HoppipPetProjectileShiny : HoppipPetProjectile { }
}
