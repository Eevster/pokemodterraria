using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WigglytuffPet
{
	public class WigglytuffPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Wigglytuff";
        public override int ProjType => ModContent.ProjectileType<WigglytuffPetProjectile>();
    }

    public class WigglytuffPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Wigglytuff";
        public override int ProjType => ModContent.ProjectileType<WigglytuffPetProjectileShiny>();
    }
}
