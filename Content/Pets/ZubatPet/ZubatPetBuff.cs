using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ZubatPet
{
	public class ZubatPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Zubat";
        public override int ProjType => ModContent.ProjectileType<ZubatPetProjectile>();
    }

    public class ZubatPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Zubat";
        public override int ProjType => ModContent.ProjectileType<ZubatPetProjectileShiny>();
    }
}
