using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.HorseaPet
{
	public class HorseaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Horsea";
        public override int ProjType => ModContent.ProjectileType<HorseaPetProjectile>();
    }

    public class HorseaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Horsea";
        public override int ProjType => ModContent.ProjectileType<HorseaPetProjectileShiny>();
    }
}
