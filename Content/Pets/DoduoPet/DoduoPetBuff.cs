using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DoduoPet
{
	public class DoduoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Doduo";
        public override int ProjType => ModContent.ProjectileType<DoduoPetProjectile>();
    }

    public class DoduoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Doduo";
        public override int ProjType => ModContent.ProjectileType<DoduoPetProjectileShiny>();
    }
}
