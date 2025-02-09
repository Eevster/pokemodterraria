using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.EeveePet
{
    public class EeveePetBuff : PokemonPetBuff
	{
        public override string PokeName => "Eevee";
        public override int ProjType => ModContent.ProjectileType<EeveePetProjectile>();
    }

    public class EeveePetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Eevee";
        public override int ProjType => ModContent.ProjectileType<EeveePetProjectileShiny>();
    }
}