using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.HypnoPet
{
	public class HypnoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Hypno";
        public override int ProjType => ModContent.ProjectileType<HypnoPetProjectile>();
    }

    public class HypnoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Hypno";
        public override int ProjType => ModContent.ProjectileType<HypnoPetProjectileShiny>();
    }
}
