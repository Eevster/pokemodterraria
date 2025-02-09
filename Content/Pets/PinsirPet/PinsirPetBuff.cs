using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PinsirPet
{
	public class PinsirPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Pinsir";
        public override int ProjType => ModContent.ProjectileType<PinsirPetProjectile>();
    }

    public class PinsirPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Pinsir";
        public override int ProjType => ModContent.ProjectileType<PinsirPetProjectileShiny>();
    }
}
