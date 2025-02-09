using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VaporeonPet
{
    public class VaporeonPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Vaporeon";
        public override int ProjType => ModContent.ProjectileType<VaporeonPetProjectile>();
    }

    public class VaporeonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Vaporeon";
        public override int ProjType => ModContent.ProjectileType<VaporeonPetProjectileShiny>();
    }
}