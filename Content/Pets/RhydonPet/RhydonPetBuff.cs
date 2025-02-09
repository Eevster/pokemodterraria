using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.RhydonPet
{
	public class RhydonPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Rhydon";
        public override int ProjType => ModContent.ProjectileType<RhydonPetProjectile>();
    }

    public class RhydonPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Rhydon";
        public override int ProjType => ModContent.ProjectileType<RhydonPetProjectileShiny>();
    }
}
