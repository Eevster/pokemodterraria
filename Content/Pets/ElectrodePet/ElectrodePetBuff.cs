using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ElectrodePet
{
	public class ElectrodePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Electrode";
        public override int ProjType => ModContent.ProjectileType<ElectrodePetProjectile>();
    }

    public class ElectrodePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Electrode";
        public override int ProjType => ModContent.ProjectileType<ElectrodePetProjectileShiny>();
    }
}
