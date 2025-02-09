using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharmanderPet
{
	public class CharmanderPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Charmander";
        public override int ProjType => ModContent.ProjectileType<CharmanderPetProjectile>();
	}

    public class CharmanderPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charmander";
        public override int ProjType => ModContent.ProjectileType<CharmanderPetProjectileShiny>();
	}
}
