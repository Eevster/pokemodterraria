using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MagnetonPet
{
	public class MagnetonPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Magneton";
        public override int ProjType => ModContent.ProjectileType<MagnetonPetProjectile>();
    }

    public class MagnetonPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Magneton";
        public override int ProjType => ModContent.ProjectileType<MagnetonPetProjectileShiny>();
    }
}
