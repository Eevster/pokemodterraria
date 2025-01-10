using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ZeraoraPet
{
	public class ZeraoraPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Zeraora";
        public override int ProjType => ModContent.ProjectileType<ZeraoraPetProjectile>();
    }

    public class ZeraoraPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Zeraora";
        public override int ProjType => ModContent.ProjectileType<ZeraoraPetProjectileShiny>();
    }
}
