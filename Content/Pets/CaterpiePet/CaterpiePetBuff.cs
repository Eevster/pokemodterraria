using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CaterpiePet
{
	public class CaterpiePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Caterpie";
        public override int ProjType => ModContent.ProjectileType<CaterpiePetProjectile>();
    }

    public class CaterpiePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Caterpie";
        public override int ProjType => ModContent.ProjectileType<CaterpiePetProjectileShiny>();
    }
}
