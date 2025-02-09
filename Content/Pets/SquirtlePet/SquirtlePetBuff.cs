using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SquirtlePet
{
	public class SquirtlePetBuff : PokemonPetBuff
	{
        public override string PokeName => "Squirtle";
        public override int ProjType => ModContent.ProjectileType<SquirtlePetProjectile>();
	}

    public class SquirtlePetBuffShiny : PokemonPetBuff
	{
		public override string PokeName => "Squirtle";
        public override int ProjType => ModContent.ProjectileType<SquirtlePetProjectileShiny>();
	}
}
