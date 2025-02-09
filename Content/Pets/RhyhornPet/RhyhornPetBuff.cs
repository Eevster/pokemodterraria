using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.RhyhornPet
{
	public class RhyhornPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Rhyhorn";
        public override int ProjType => ModContent.ProjectileType<RhyhornPetProjectile>();
    }

    public class RhyhornPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Rhyhorn";
        public override int ProjType => ModContent.ProjectileType<RhyhornPetProjectileShiny>();
    }
}
