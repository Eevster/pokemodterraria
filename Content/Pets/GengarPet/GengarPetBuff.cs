using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GengarPet
{
	public class GengarPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Gengar";
        public override int ProjType => ModContent.ProjectileType<GengarPetProjectile>();
    }

    public class GengarPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Gengar";
        public override int ProjType => ModContent.ProjectileType<GengarPetProjectileShiny>();
    }
}
