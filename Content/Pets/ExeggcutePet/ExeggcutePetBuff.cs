using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ExeggcutePet
{
	public class ExeggcutePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Exeggcute";
        public override int ProjType => ModContent.ProjectileType<ExeggcutePetProjectile>();
    }

    public class ExeggcutePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Exeggcute";
        public override int ProjType => ModContent.ProjectileType<ExeggcutePetProjectileShiny>();
    }
}
