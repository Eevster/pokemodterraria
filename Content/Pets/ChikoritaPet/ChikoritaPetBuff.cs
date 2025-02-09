using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ChikoritaPet
{
	public class ChikoritaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Chikorita";
        public override int ProjType => ModContent.ProjectileType<ChikoritaPetProjectile>();
    }

    public class ChikoritaPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Chikorita";
        public override int ProjType => ModContent.ProjectileType<ChikoritaPetProjectileShiny>();
    }
}
