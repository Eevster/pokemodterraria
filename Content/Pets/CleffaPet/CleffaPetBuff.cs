using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CleffaPet
{
	public class CleffaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Cleffa";
        public override int ProjType => ModContent.ProjectileType<CleffaPetProjectile>();
    }

    public class CleffaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Cleffa";
        public override int ProjType => ModContent.ProjectileType<CleffaPetProjectileShiny>();
    }
}
