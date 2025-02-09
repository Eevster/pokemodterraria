using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PorygonPet
{
	public class PorygonPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Porygon";
        public override int ProjType => ModContent.ProjectileType<PorygonPetProjectile>();
    }

    public class PorygonPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Porygon";
        public override int ProjType => ModContent.ProjectileType<PorygonPetProjectileShiny>();
    }
}
