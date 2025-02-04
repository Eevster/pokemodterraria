using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.LaprasPet
{
	public class LaprasPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Lapras";
        public override int ProjType => ModContent.ProjectileType<LaprasPetProjectile>();
    }

    public class LaprasPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Lapras";
        public override int ProjType => ModContent.ProjectileType<LaprasPetProjectileShiny>();
    }
}
