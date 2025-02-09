using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharmeleonPet
{
	public class CharmeleonPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Charmeleon";
        public override int ProjType => ModContent.ProjectileType<CharmeleonPetProjectile>();
	}

    public class CharmeleonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charmeleon";
        public override int ProjType => ModContent.ProjectileType<CharmeleonPetProjectileShiny>();
	}
}
