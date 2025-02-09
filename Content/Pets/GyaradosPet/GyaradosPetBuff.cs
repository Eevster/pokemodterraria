using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GyaradosPet
{
	public class GyaradosPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Gyarados";
        public override int ProjType => ModContent.ProjectileType<GyaradosPetProjectile>();
    }

    public class GyaradosPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Gyarados";
        public override int ProjType => ModContent.ProjectileType<GyaradosPetProjectileShiny>();
    }
}
