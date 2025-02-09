using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SandslashPet
{
	public class SandslashPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Sandslash";
        public override int ProjType => ModContent.ProjectileType<SandslashPetProjectile>();
    }

    public class SandslashPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Sandslash";
        public override int ProjType => ModContent.ProjectileType<SandslashPetProjectileShiny>();
    }
}
