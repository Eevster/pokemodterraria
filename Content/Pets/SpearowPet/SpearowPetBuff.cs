using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SpearowPet
{
	public class SpearowPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Spearow";
        public override int ProjType => ModContent.ProjectileType<SpearowPetProjectile>();
    }

    public class SpearowPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Spearow";
        public override int ProjType => ModContent.ProjectileType<SpearowPetProjectileShiny>();
    }
}
