using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CyndaquilPet
{
	public class CyndaquilPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Cyndaquil";
        public override int ProjType => ModContent.ProjectileType<CyndaquilPetProjectile>();
    }

    public class CyndaquilPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Cyndaquil";
        public override int ProjType => ModContent.ProjectileType<CyndaquilPetProjectileShiny>();
    }
}
