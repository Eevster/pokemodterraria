using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VenusaurPet
{
	public class VenusaurPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Venusaur";
        public override int ProjType => ModContent.ProjectileType<VenusaurPetProjectile>();
	}

    public class VenusaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Venusaur";
        public override int ProjType => ModContent.ProjectileType<VenusaurPetProjectileShiny>();
	}
}
