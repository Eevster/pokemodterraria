using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SeadraPet
{
	public class SeadraPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Seadra";
        public override int ProjType => ModContent.ProjectileType<SeadraPetProjectile>();
    }

    public class SeadraPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Seadra";
        public override int ProjType => ModContent.ProjectileType<SeadraPetProjectileShiny>();
    }
}
