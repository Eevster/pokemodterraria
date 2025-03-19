using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidoranFPet
{
	public class NidoranFPetBuff: PokemonPetBuff
	{
        public override string PokeName => "NidoranF";
        public override int ProjType => ModContent.ProjectileType<NidoranFPetProjectile>();
    }

    public class NidoranFPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "NidoranF";
        public override int ProjType => ModContent.ProjectileType<NidoranFPetProjectileShiny>();
    }
}
