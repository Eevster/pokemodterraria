using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GrowlithePet
{
	public class GrowlithePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Growlithe";
        public override int ProjType => ModContent.ProjectileType<GrowlithePetProjectile>();
    }

    public class GrowlithePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Growlithe";
        public override int ProjType => ModContent.ProjectileType<GrowlithePetProjectileShiny>();
    }
}
