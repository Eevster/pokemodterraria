using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KabutopsPet
{
	public class KabutopsPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kabutops";
        public override int ProjType => ModContent.ProjectileType<KabutopsPetProjectile>();
    }

    public class KabutopsPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kabutops";
        public override int ProjType => ModContent.ProjectileType<KabutopsPetProjectileShiny>();
    }
}
