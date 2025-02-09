using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TentacoolPet
{
	public class TentacoolPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Tentacool";
        public override int ProjType => ModContent.ProjectileType<TentacoolPetProjectile>();
    }

    public class TentacoolPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Tentacool";
        public override int ProjType => ModContent.ProjectileType<TentacoolPetProjectileShiny>();
    }
}
