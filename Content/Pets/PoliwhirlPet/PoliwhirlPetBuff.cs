using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PoliwhirlPet
{
	public class PoliwhirlPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Poliwhirl";
        public override int ProjType => ModContent.ProjectileType<PoliwhirlPetProjectile>();
    }

    public class PoliwhirlPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Poliwhirl";
        public override int ProjType => ModContent.ProjectileType<PoliwhirlPetProjectileShiny>();
    }
}
