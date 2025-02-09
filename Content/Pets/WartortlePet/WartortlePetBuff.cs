using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WartortlePet
{
	public class WartortlePetBuff : PokemonPetBuff
	{
        public override string PokeName => "Wartortle";
        public override int ProjType => ModContent.ProjectileType<WartortlePetProjectile>();
	}

    public class WartortlePetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Wartortle";
        public override int ProjType => ModContent.ProjectileType<WartortlePetProjectileShiny>();
	}
}
