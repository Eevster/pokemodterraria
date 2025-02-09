using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.FarfetchdPet
{
	public class FarfetchdPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Farfetchd";
        public override int ProjType => ModContent.ProjectileType<FarfetchdPetProjectile>();
    }

    public class FarfetchdPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Farfetchd";
        public override int ProjType => ModContent.ProjectileType<FarfetchdPetProjectileShiny>();
    }
}
