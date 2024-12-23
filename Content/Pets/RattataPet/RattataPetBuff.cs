using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.RattataPet
{
	public class RattataPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Rattata";
        public override int ProjType => ModContent.ProjectileType<RattataPetProjectile>();
    }

    public class RattataPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Rattata";
        public override int ProjType => ModContent.ProjectileType<RattataPetProjectileShiny>();
    }
}
