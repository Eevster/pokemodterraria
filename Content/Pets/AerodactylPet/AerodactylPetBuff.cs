using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.AerodactylPet
{
	public class AerodactylPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Aerodactyl";
        public override int ProjType => ModContent.ProjectileType<AerodactylPetProjectile>();
    }

    public class AerodactylPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Aerodactyl";
        public override int ProjType => ModContent.ProjectileType<AerodactylPetProjectileShiny>();
    }
}
