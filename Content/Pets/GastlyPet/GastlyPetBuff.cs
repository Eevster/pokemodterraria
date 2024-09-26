using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GastlyPet
{
	public class GastlyPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Gastly";
        public override int ProjType => ModContent.ProjectileType<GastlyPetProjectile>();
    }

    public class GastlyPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Gastly";
        public override int ProjType => ModContent.ProjectileType<GastlyPetProjectileShiny>();
    }
}
