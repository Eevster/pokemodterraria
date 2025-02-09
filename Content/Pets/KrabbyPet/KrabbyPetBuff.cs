using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KrabbyPet
{
	public class KrabbyPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Krabby";
        public override int ProjType => ModContent.ProjectileType<KrabbyPetProjectile>();
    }

    public class KrabbyPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Krabby";
        public override int ProjType => ModContent.ProjectileType<KrabbyPetProjectileShiny>();
    }
}
