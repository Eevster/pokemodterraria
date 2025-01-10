using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ChanseyPet
{
	public class ChanseyPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Chansey";
        public override int ProjType => ModContent.ProjectileType<ChanseyPetProjectile>();
    }

    public class ChanseyPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Chansey";
        public override int ProjType => ModContent.ProjectileType<ChanseyPetProjectileShiny>();
    }
}
