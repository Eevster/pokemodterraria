using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.OmanytePet
{
	public class OmanytePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Omanyte";
        public override int ProjType => ModContent.ProjectileType<OmanytePetProjectile>();
    }

    public class OmanytePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Omanyte";
        public override int ProjType => ModContent.ProjectileType<OmanytePetProjectileShiny>();
    }
}
