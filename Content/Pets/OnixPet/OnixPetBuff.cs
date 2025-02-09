using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.OnixPet
{
	public class OnixPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Onix";
        public override int ProjType => ModContent.ProjectileType<OnixPetProjectile>();
    }

    public class OnixPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Onix";
        public override int ProjType => ModContent.ProjectileType<OnixPetProjectileShiny>();
    }
}
