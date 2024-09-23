using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WeedlePet
{
	public class WeedlePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Weedle";
        public override int ProjType => ModContent.ProjectileType<WeedlePetProjectile>();
    }

    public class WeedlePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Weedle";
        public override int ProjType => ModContent.ProjectileType<WeedlePetProjectileShiny>();
    }
}
