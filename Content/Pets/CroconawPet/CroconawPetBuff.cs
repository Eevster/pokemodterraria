using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CroconawPet
{
	public class CroconawPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Croconaw";
        public override int ProjType => ModContent.ProjectileType<CroconawPetProjectile>();
    }

    public class CroconawPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Croconaw";
        public override int ProjType => ModContent.ProjectileType<CroconawPetProjectileShiny>();
    }
}
