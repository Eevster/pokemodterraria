using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharizardPet
{
	public class CharizardPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Charizard";
        public override int ProjType => ModContent.ProjectileType<CharizardPetProjectile>();
	}

    public class CharizardPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charizard";
        public override int ProjType => ModContent.ProjectileType<CharizardPetProjectileShiny>();
	}
}
