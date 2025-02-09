using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ScytherPet
{
	public class ScytherPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Scyther";
        public override int ProjType => ModContent.ProjectileType<ScytherPetProjectile>();
    }

    public class ScytherPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Scyther";
        public override int ProjType => ModContent.ProjectileType<ScytherPetProjectileShiny>();
    }
}
