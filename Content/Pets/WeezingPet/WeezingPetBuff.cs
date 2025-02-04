using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WeezingPet
{
	public class WeezingPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Weezing";
        public override int ProjType => ModContent.ProjectileType<WeezingPetProjectile>();
    }

    public class WeezingPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Weezing";
        public override int ProjType => ModContent.ProjectileType<WeezingPetProjectileShiny>();
    }
}
