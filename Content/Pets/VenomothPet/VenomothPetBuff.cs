using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VenomothPet
{
	public class VenomothPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Venomoth";
        public override int ProjType => ModContent.ProjectileType<VenomothPetProjectile>();
    }

    public class VenomothPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Venomoth";
        public override int ProjType => ModContent.ProjectileType<VenomothPetProjectileShiny>();
    }
}
