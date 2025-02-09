using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TaurosPet
{
	public class TaurosPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Tauros";
        public override int ProjType => ModContent.ProjectileType<TaurosPetProjectile>();
    }

    public class TaurosPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Tauros";
        public override int ProjType => ModContent.ProjectileType<TaurosPetProjectileShiny>();
    }
}
