using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SandshrewPet
{
	public class SandshrewPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Sandshrew";
        public override int ProjType => ModContent.ProjectileType<SandshrewPetProjectile>();
    }

    public class SandshrewPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Sandshrew";
        public override int ProjType => ModContent.ProjectileType<SandshrewPetProjectileShiny>();
    }
}
