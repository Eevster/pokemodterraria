using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SnorlaxPet
{
	public class SnorlaxPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Snorlax";
        public override int ProjType => ModContent.ProjectileType<SnorlaxPetProjectile>();
    }

    public class SnorlaxPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Snorlax";
        public override int ProjType => ModContent.ProjectileType<SnorlaxPetProjectileShiny>();
    }
}
