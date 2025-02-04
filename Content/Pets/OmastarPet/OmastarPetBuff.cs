using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.OmastarPet
{
	public class OmastarPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Omastar";
        public override int ProjType => ModContent.ProjectileType<OmastarPetProjectile>();
    }

    public class OmastarPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Omastar";
        public override int ProjType => ModContent.ProjectileType<OmastarPetProjectileShiny>();
    }
}
