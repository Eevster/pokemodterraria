using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VoltorbPet
{
	public class VoltorbPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Voltorb";
        public override int ProjType => ModContent.ProjectileType<VoltorbPetProjectile>();
    }

    public class VoltorbPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Voltorb";
        public override int ProjType => ModContent.ProjectileType<VoltorbPetProjectileShiny>();
    }
}
