using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.AlakazamPet
{
	public class AlakazamPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 44;
        public override int hitboxHeight => 46;

        public override int totalFrames => 14;
        public override int animationSpeed => 7;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 8];
        public override int[] jumpStartEnd => [9, 9];
        public override int[] fallStartEnd => [9, 9];
        public override int[] attackStartEnd => [9, 13];

        public override string[] megaEvolutions => ["AlakazamMega"];
        public override string[] itemToMegaEvolve => ["AlakazamMegaStoneItem"];
    }

	public class AlakazamPetProjectileShiny : AlakazamPetProjectile{}
}
