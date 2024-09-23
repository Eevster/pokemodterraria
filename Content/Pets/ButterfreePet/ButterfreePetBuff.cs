using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ButterfreePet
{
	public class ButterfreePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Butterfree";
        public override int ProjType => ModContent.ProjectileType<ButterfreePetProjectile>();
    }

    public class ButterfreePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Butterfree";
        public override int ProjType => ModContent.ProjectileType<ButterfreePetProjectileShiny>();
    }
}
