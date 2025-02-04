using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PoliwagPet
{
	public class PoliwagPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Poliwag";
        public override int ProjType => ModContent.ProjectileType<PoliwagPetProjectile>();
    }

    public class PoliwagPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Poliwag";
        public override int ProjType => ModContent.ProjectileType<PoliwagPetProjectileShiny>();
    }
}
