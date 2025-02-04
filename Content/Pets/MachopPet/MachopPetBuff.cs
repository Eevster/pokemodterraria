using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MachopPet
{
	public class MachopPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Machop";
        public override int ProjType => ModContent.ProjectileType<MachopPetProjectile>();
    }

    public class MachopPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Machop";
        public override int ProjType => ModContent.ProjectileType<MachopPetProjectileShiny>();
    }
}
