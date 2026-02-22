using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AlolanSandshrewPet
{
	public class AlolanSandshrewPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 28;
        public override int hitboxHeight => 36;

        public override int totalFrames => 20;
        public override int animationSpeed => 6;
        public override int[] idleStartEnd => [0, 6];
        public override int[] walkStartEnd => [7, 14];
        public override int[] jumpStartEnd => [12, 12];
        public override int[] fallStartEnd => [8, 8];
        public override int[] attackStartEnd => [15, 19];

        public override string[] evolutions => ["AlolanSandslash"];
        public override string[] itemToEvolve => ["IceStoneItem"];
    }

	public class AlolanSandshrewPetProjectileShiny : AlolanSandshrewPetProjectile { }
}
