using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GeodudePet
{
	public class GeodudePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Geodude";
        public override int ProjType => ModContent.ProjectileType<GeodudePetProjectile>();
    }

    public class GeodudePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Geodude";
        public override int ProjType => ModContent.ProjectileType<GeodudePetProjectileShiny>();
    }
}
