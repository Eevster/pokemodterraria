using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DratiniPet
{
	public class DratiniPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dratini";
        public override int ProjType => ModContent.ProjectileType<DratiniPetProjectile>();
    }

    public class DratiniPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dratini";
        public override int ProjType => ModContent.ProjectileType<DratiniPetProjectileShiny>();
    }
}
