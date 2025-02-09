using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ClefablePet
{
	public class ClefablePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Clefable";
        public override int ProjType => ModContent.ProjectileType<ClefablePetProjectile>();
    }

    public class ClefablePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Clefable";
        public override int ProjType => ModContent.ProjectileType<ClefablePetProjectileShiny>();
    }
}
