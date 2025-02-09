using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PrimeapePet
{
	public class PrimeapePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Primeape";
        public override int ProjType => ModContent.ProjectileType<PrimeapePetProjectile>();
    }

    public class PrimeapePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Primeape";
        public override int ProjType => ModContent.ProjectileType<PrimeapePetProjectileShiny>();
    }
}
