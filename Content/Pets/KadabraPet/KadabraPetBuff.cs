using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KadabraPet
{
	public class KadabraPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kadabra";
        public override int ProjType => ModContent.ProjectileType<KadabraPetProjectile>();
    }

    public class KadabraPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kadabra";
        public override int ProjType => ModContent.ProjectileType<KadabraPetProjectileShiny>();
    }
}
