using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidorinaPet
{
	public class NidorinaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Nidorina";
        public override int ProjType => ModContent.ProjectileType<NidorinaPetProjectile>();
    }

    public class NidorinaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Nidorina";
        public override int ProjType => ModContent.ProjectileType<NidorinaPetProjectileShiny>();
    }
}
