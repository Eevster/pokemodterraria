using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MagnemitePet
{
	public class MagnemitePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Magnemite";
        public override int ProjType => ModContent.ProjectileType<MagnemitePetProjectile>();
    }

    public class MagnemitePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Magnemite";
        public override int ProjType => ModContent.ProjectileType<MagnemitePetProjectileShiny>();
    }
}
