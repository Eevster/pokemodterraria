using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SlowbroPet
{
	public class SlowbroPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Slowbro";
        public override int ProjType => ModContent.ProjectileType<SlowbroPetProjectile>();
    }

    public class SlowbroPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Slowbro";
        public override int ProjType => ModContent.ProjectileType<SlowbroPetProjectileShiny>();
    }
}
