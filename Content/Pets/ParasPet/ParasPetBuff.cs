using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ParasPet
{
	public class ParasPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Paras";
        public override int ProjType => ModContent.ProjectileType<ParasPetProjectile>();
    }

    public class ParasPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Paras";
        public override int ProjType => ModContent.ProjectileType<ParasPetProjectileShiny>();
    }
}
