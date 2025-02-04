using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ArcaninePet
{
	public class ArcaninePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Arcanine";
        public override int ProjType => ModContent.ProjectileType<ArcaninePetProjectile>();
    }

    public class ArcaninePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Arcanine";
        public override int ProjType => ModContent.ProjectileType<ArcaninePetProjectileShiny>();
    }
}
