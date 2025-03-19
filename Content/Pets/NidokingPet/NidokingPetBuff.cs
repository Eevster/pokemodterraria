using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NidokingPet
{
	public class NidokingPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Nidoking";
        public override int ProjType => ModContent.ProjectileType<NidokingPetProjectile>();
    }

    public class NidokingPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Nidoking";
        public override int ProjType => ModContent.ProjectileType<NidokingPetProjectileShiny>();
    }
}
