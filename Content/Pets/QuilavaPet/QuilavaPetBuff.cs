using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.QuilavaPet
{
	public class QuilavaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Quilava";
        public override int ProjType => ModContent.ProjectileType<QuilavaPetProjectile>();
    }

    public class QuilavaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Quilava";
        public override int ProjType => ModContent.ProjectileType<QuilavaPetProjectileShiny>();
    }
}
