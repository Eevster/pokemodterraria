using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TangelaPet
{
	public class TangelaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Tangela";
        public override int ProjType => ModContent.ProjectileType<TangelaPetProjectile>();
    }

    public class TangelaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Tangela";
        public override int ProjType => ModContent.ProjectileType<TangelaPetProjectileShiny>();
    }
}
