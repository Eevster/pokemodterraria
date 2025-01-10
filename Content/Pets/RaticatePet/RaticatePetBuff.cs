using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.RaticatePet
{
	public class RaticatePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Raticate";
        public override int ProjType => ModContent.ProjectileType<RaticatePetProjectile>();
    }

    public class RaticatePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Raticate";
        public override int ProjType => ModContent.ProjectileType<RaticatePetProjectileShiny>();
    }
}
