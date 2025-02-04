using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ZapdosPet
{
	public class ZapdosPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Zapdos";
        public override int ProjType => ModContent.ProjectileType<ZapdosPetProjectile>();
    }

    public class ZapdosPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Zapdos";
        public override int ProjType => ModContent.ProjectileType<ZapdosPetProjectileShiny>();
    }
}
