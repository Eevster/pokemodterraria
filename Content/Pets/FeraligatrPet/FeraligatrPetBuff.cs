using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.FeraligatrPet
{
	public class FeraligatrPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Feraligatr";
        public override int ProjType => ModContent.ProjectileType<FeraligatrPetProjectile>();
    }

    public class FeraligatrPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Feraligatr";
        public override int ProjType => ModContent.ProjectileType<FeraligatrPetProjectileShiny>();
    }
}
