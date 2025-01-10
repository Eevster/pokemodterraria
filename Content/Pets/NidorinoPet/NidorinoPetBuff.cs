using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidorinoPet
{
	public class NidorinoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Nidorino";
        public override int ProjType => ModContent.ProjectileType<NidorinoPetProjectile>();
    }

    public class NidorinoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Nidorino";
        public override int ProjType => ModContent.ProjectileType<NidorinoPetProjectileShiny>();
    }
}
