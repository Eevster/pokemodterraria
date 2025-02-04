using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CubonePet
{
	public class CubonePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Cubone";
        public override int ProjType => ModContent.ProjectileType<CubonePetProjectile>();
    }

    public class CubonePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Cubone";
        public override int ProjType => ModContent.ProjectileType<CubonePetProjectileShiny>();
    }
}
