using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PersianPet
{
	public class PersianPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Persian";
        public override int ProjType => ModContent.ProjectileType<PersianPetProjectile>();
    }

    public class PersianPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Persian";
        public override int ProjType => ModContent.ProjectileType<PersianPetProjectileShiny>();
    }
}
