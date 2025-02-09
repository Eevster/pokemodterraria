using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SeakingPet
{
	public class SeakingPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Seaking";
        public override int ProjType => ModContent.ProjectileType<SeakingPetProjectile>();
    }

    public class SeakingPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Seaking";
        public override int ProjType => ModContent.ProjectileType<SeakingPetProjectileShiny>();
    }
}
