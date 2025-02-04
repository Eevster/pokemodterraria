using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.StaryuPet
{
	public class StaryuPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Staryu";
        public override int ProjType => ModContent.ProjectileType<StaryuPetProjectile>();
    }

    public class StaryuPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Staryu";
        public override int ProjType => ModContent.ProjectileType<StaryuPetProjectileShiny>();
    }
}
