using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TentacruelPet
{
	public class TentacruelPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Tentacruel";
        public override int ProjType => ModContent.ProjectileType<TentacruelPetProjectile>();
    }

    public class TentacruelPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Tentacruel";
        public override int ProjType => ModContent.ProjectileType<TentacruelPetProjectileShiny>();
    }
}
