using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.EkansPet
{
	public class EkansPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Ekans";
        public override int ProjType => ModContent.ProjectileType<EkansPetProjectile>();
    }

    public class EkansPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Ekans";
        public override int ProjType => ModContent.ProjectileType<EkansPetProjectileShiny>();
    }
}
