using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ArbokPet
{
	public class ArbokPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Arbok";
        public override int ProjType => ModContent.ProjectileType<ArbokPetProjectile>();
    }

    public class ArbokPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Arbok";
        public override int ProjType => ModContent.ProjectileType<ArbokPetProjectileShiny>();
    }
}
