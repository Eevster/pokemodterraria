using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.NinetalesPet
{
	public class NinetalesPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Ninetales";
        public override int ProjType => ModContent.ProjectileType<NinetalesPetProjectile>();
    }

    public class NinetalesPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Ninetales";
        public override int ProjType => ModContent.ProjectileType<NinetalesPetProjectileShiny>();
    }
}
