using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ShellderPet
{
	public class ShellderPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Shellder";
        public override int ProjType => ModContent.ProjectileType<ShellderPetProjectile>();
    }

    public class ShellderPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Shellder";
        public override int ProjType => ModContent.ProjectileType<ShellderPetProjectileShiny>();
    }
}
