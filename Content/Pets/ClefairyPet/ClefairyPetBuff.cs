using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ClefairyPet
{
	public class ClefairyPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Clefairy";
        public override int ProjType => ModContent.ProjectileType<ClefairyPetProjectile>();
    }

    public class ClefairyPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Clefairy";
        public override int ProjType => ModContent.ProjectileType<ClefairyPetProjectileShiny>();
    }
}
