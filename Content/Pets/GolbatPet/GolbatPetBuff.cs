using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GolbatPet
{
	public class GolbatPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Golbat";
        public override int ProjType => ModContent.ProjectileType<GolbatPetProjectile>();
    }

    public class GolbatPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Golbat";
        public override int ProjType => ModContent.ProjectileType<GolbatPetProjectileShiny>();
    }
}
