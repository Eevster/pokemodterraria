using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SlowkingPet
{
	public class SlowkingPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Slowking";
        public override int ProjType => ModContent.ProjectileType<SlowkingPetProjectile>();
    }

    public class SlowkingPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Slowking";
        public override int ProjType => ModContent.ProjectileType<SlowkingPetProjectileShiny>();
    }
}
