using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DittoPet
{
	public class DittoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Ditto";
        public override int ProjType => ModContent.ProjectileType<DittoPetProjectile>();
    }

    public class DittoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Ditto";
        public override int ProjType => ModContent.ProjectileType<DittoPetProjectileShiny>();
    }
}
