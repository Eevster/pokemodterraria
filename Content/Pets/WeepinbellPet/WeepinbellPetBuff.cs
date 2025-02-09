using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WeepinbellPet
{
	public class WeepinbellPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Weepinbell";
        public override int ProjType => ModContent.ProjectileType<WeepinbellPetProjectile>();
    }

    public class WeepinbellPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Weepinbell";
        public override int ProjType => ModContent.ProjectileType<WeepinbellPetProjectileShiny>();
    }
}
