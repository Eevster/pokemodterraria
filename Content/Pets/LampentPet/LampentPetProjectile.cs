using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.LampentPet
{
	public class LampentPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 38;
        public override int hitboxHeight => 38;

        public override int moveStyle => 1;

        public override int totalFrames => 19;
        public override int animationSpeed => 11;
        public override int[] idleStartEnd => [9, 18];
        public override int[] walkStartEnd => [6, 8];
        public override int[] idleFlyStartEnd => [9, 18];
        public override int[] walkFlyStartEnd => [6, 8];
        public override int[] attackFlyStartEnd => [0, 5];

        public override string[] evolutions => ["Chandelure"];
        public override string[] itemToEvolve => ["DuskStoneItem"];

	}

	public class LampentPetProjectileShiny : LampentPetProjectile{}
}
