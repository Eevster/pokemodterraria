using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MachampPet
{
	public class MachampPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Machamp";
        public override int ProjType => ModContent.ProjectileType<MachampPetProjectile>();
    }

    public class MachampPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Machamp";
        public override int ProjType => ModContent.ProjectileType<MachampPetProjectileShiny>();
    }
}
