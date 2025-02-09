using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.OddishPet
{
	public class OddishPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Oddish";
        public override int ProjType => ModContent.ProjectileType<OddishPetProjectile>();
    }

    public class OddishPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Oddish";
        public override int ProjType => ModContent.ProjectileType<OddishPetProjectileShiny>();
    }
}
