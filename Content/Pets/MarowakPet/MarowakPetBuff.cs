using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MarowakPet
{
	public class MarowakPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Marowak";
        public override int ProjType => ModContent.ProjectileType<MarowakPetProjectile>();
    }

    public class MarowakPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Marowak";
        public override int ProjType => ModContent.ProjectileType<MarowakPetProjectileShiny>();
    }
}
