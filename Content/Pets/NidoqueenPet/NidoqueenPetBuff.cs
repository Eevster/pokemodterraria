using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidoqueenPet
{
	public class NidoqueenPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Nidoqueen";
        public override int ProjType => ModContent.ProjectileType<NidoqueenPetProjectile>();
    }

    public class NidoqueenPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Nidoqueen";
        public override int ProjType => ModContent.ProjectileType<NidoqueenPetProjectileShiny>();
    }
}
