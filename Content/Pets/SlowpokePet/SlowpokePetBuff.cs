using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SlowpokePet
{
	public class SlowpokePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Slowpoke";
        public override int ProjType => ModContent.ProjectileType<SlowpokePetProjectile>();
    }

    public class SlowpokePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Slowpoke";
        public override int ProjType => ModContent.ProjectileType<SlowpokePetProjectileShiny>();
    }
}
