using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GolduckPet
{
	public class GolduckPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Golduck";
        public override int ProjType => ModContent.ProjectileType<GolduckPetProjectile>();
    }

    public class GolduckPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Golduck";
        public override int ProjType => ModContent.ProjectileType<GolduckPetProjectileShiny>();
    }
}
