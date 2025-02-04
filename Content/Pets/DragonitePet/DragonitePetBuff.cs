using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DragonitePet
{
	public class DragonitePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dragonite";
        public override int ProjType => ModContent.ProjectileType<DragonitePetProjectile>();
    }

    public class DragonitePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dragonite";
        public override int ProjType => ModContent.ProjectileType<DragonitePetProjectileShiny>();
    }
}
