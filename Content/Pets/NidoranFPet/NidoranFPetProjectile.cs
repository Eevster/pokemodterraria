using Microsoft.Xna.Framework;
using Pokemod.Content.Projectiles.PokemonAttackProjs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Pets.NidoranFPet
{
	public class NidoranFPetProjectile : PokemonPetProjectile
	{
        public override int hitboxWidth => 32;
        public override int hitboxHeight => 24;

        public override int totalFrames => 16;
        public override int animationSpeed => 5;
        public override int[] idleStartEnd => [0, 4];
        public override int[] walkStartEnd => [5, 9];
        public override int[] jumpStartEnd => [7, 7];
        public override int[] fallStartEnd => [9, 9];
        public override int[] attackStartEnd => [10, 15];

        public override string[] evolutions => ["Nidorina"];
        public override int levelToEvolve => 16;
        public override int levelEvolutionsNumber => 1;
    }

	public class NidoranFPetProjectileShiny : NidoranFPetProjectile{}
}
