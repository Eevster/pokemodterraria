using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MachokePet
{
	public class MachokePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Machoke";
        public override int ProjType => ModContent.ProjectileType<MachokePetProjectile>();
    }

    public class MachokePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Machoke";
        public override int ProjType => ModContent.ProjectileType<MachokePetProjectileShiny>();
    }
}
