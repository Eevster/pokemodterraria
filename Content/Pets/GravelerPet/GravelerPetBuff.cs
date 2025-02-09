using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GravelerPet
{
	public class GravelerPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Graveler";
        public override int ProjType => ModContent.ProjectileType<GravelerPetProjectile>();
    }

    public class GravelerPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Graveler";
        public override int ProjType => ModContent.ProjectileType<GravelerPetProjectileShiny>();
    }
}
