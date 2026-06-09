using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.QuagsirePet
{
	public class QuagsirePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Quagsire";
        public override int ProjType => ModContent.ProjectileType<QuagsirePetProjectile>();
    }

    public class QuagsirePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Quagsire";
        public override int ProjType => ModContent.ProjectileType<QuagsirePetProjectileShiny>();
    }
}
