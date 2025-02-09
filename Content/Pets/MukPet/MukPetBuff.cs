using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MukPet
{
	public class MukPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Muk";
        public override int ProjType => ModContent.ProjectileType<MukPetProjectile>();
    }

    public class MukPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Muk";
        public override int ProjType => ModContent.ProjectileType<MukPetProjectileShiny>();
    }
}
