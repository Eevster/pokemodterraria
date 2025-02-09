using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ElectabuzzPet
{
	public class ElectabuzzPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Electabuzz";
        public override int ProjType => ModContent.ProjectileType<ElectabuzzPetProjectile>();
    }

    public class ElectabuzzPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Electabuzz";
        public override int ProjType => ModContent.ProjectileType<ElectabuzzPetProjectileShiny>();
    }
}
