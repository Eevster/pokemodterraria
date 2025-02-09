using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DewgongPet
{
	public class DewgongPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dewgong";
        public override int ProjType => ModContent.ProjectileType<DewgongPetProjectile>();
    }

    public class DewgongPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dewgong";
        public override int ProjType => ModContent.ProjectileType<DewgongPetProjectileShiny>();
    }
}
