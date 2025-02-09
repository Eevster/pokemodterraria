using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KabutoPet
{
	public class KabutoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kabuto";
        public override int ProjType => ModContent.ProjectileType<KabutoPetProjectile>();
    }

    public class KabutoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kabuto";
        public override int ProjType => ModContent.ProjectileType<KabutoPetProjectileShiny>();
    }
}
