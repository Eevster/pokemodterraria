using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DiglettPet
{
	public class DiglettPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Diglett";
        public override int ProjType => ModContent.ProjectileType<DiglettPetProjectile>();
    }

    public class DiglettPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Diglett";
        public override int ProjType => ModContent.ProjectileType<DiglettPetProjectileShiny>();
    }
}
