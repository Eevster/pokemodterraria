using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PoliwrathPet
{
	public class PoliwrathPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Poliwrath";
        public override int ProjType => ModContent.ProjectileType<PoliwrathPetProjectile>();
    }

    public class PoliwrathPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Poliwrath";
        public override int ProjType => ModContent.ProjectileType<PoliwrathPetProjectileShiny>();
    }
}
