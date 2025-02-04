using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VulpixPet
{
	public class VulpixPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Vulpix";
        public override int ProjType => ModContent.ProjectileType<VulpixPetProjectile>();
    }

    public class VulpixPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Vulpix";
        public override int ProjType => ModContent.ProjectileType<VulpixPetProjectileShiny>();
    }
}
