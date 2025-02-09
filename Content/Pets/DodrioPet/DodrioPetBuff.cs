using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DodrioPet
{
	public class DodrioPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dodrio";
        public override int ProjType => ModContent.ProjectileType<DodrioPetProjectile>();
    }

    public class DodrioPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dodrio";
        public override int ProjType => ModContent.ProjectileType<DodrioPetProjectileShiny>();
    }
}
