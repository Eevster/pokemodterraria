using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.JigglypuffPet
{
	public class JigglypuffPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 24;
        public override int hitboxHeight => 32;

        public override int totalFrames => 17;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [7, 12];
        public override int[] jumpStartEnd => [13, 13];
        public override int[] fallStartEnd => [7, 7];
        public override int[] attackStartEnd => [13, 16];

        public override string[] evolutions => ["Wigglytuff"];
        public override string[] itemToEvolve => ["MoonStoneItem"];
	}

	public class JigglypuffPetProjectileShiny : JigglypuffPetProjectile{}
}
