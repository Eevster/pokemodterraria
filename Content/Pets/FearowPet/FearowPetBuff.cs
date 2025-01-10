using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.FearowPet
{
	public class FearowPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Fearow";
        public override int ProjType => ModContent.ProjectileType<FearowPetProjectile>();
    }

    public class FearowPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Fearow";
        public override int ProjType => ModContent.ProjectileType<FearowPetProjectileShiny>();
    }
}
