using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ArticunoPet
{
	public class ArticunoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Articuno";
        public override int ProjType => ModContent.ProjectileType<ArticunoPetProjectile>();
    }

    public class ArticunoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Articuno";
        public override int ProjType => ModContent.ProjectileType<ArticunoPetProjectileShiny>();
    }
}
