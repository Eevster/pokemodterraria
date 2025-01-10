using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidoranMPet
{
	public class NidoranMPetBuff: PokemonPetBuff
	{
        public override string PokeName => "NidoranM";
        public override int ProjType => ModContent.ProjectileType<NidoranMPetProjectile>();
    }

    public class NidoranMPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "NidoranM";
        public override int ProjType => ModContent.ProjectileType<NidoranMPetProjectileShiny>();
    }
}
