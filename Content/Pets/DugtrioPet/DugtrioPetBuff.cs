using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DugtrioPet
{
	public class DugtrioPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dugtrio";
        public override int ProjType => ModContent.ProjectileType<DugtrioPetProjectile>();
    }

    public class DugtrioPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dugtrio";
        public override int ProjType => ModContent.ProjectileType<DugtrioPetProjectileShiny>();
    }
}
