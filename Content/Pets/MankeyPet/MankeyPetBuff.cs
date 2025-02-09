using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MankeyPet
{
	public class MankeyPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Mankey";
        public override int ProjType => ModContent.ProjectileType<MankeyPetProjectile>();
    }

    public class MankeyPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Mankey";
        public override int ProjType => ModContent.ProjectileType<MankeyPetProjectileShiny>();
    }
}
