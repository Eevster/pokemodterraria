using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TotodilePet
{
	public class TotodilePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Totodile";
        public override int ProjType => ModContent.ProjectileType<TotodilePetProjectile>();
    }

    public class TotodilePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Totodile";
        public override int ProjType => ModContent.ProjectileType<TotodilePetProjectileShiny>();
    }
}
