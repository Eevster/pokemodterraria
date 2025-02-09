using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GoldeenPet
{
	public class GoldeenPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Goldeen";
        public override int ProjType => ModContent.ProjectileType<GoldeenPetProjectile>();
    }

    public class GoldeenPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Goldeen";
        public override int ProjType => ModContent.ProjectileType<GoldeenPetProjectileShiny>();
    }
}
