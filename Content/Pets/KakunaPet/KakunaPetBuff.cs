using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KakunaPet
{
	public class KakunaPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kakuna";
        public override int ProjType => ModContent.ProjectileType<KakunaPetProjectile>();
    }

    public class KakunaPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kakuna";
        public override int ProjType => ModContent.ProjectileType<KakunaPetProjectileShiny>();
    }
}
