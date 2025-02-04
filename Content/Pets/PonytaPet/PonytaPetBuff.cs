using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PonytaPet
{
	public class PonytaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Ponyta";
        public override int ProjType => ModContent.ProjectileType<PonytaPetProjectile>();
    }

    public class PonytaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Ponyta";
        public override int ProjType => ModContent.ProjectileType<PonytaPetProjectileShiny>();
    }
}
