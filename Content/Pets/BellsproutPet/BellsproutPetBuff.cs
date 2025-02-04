using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BellsproutPet
{
	public class BellsproutPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Bellsprout";
        public override int ProjType => ModContent.ProjectileType<BellsproutPetProjectile>();
    }

    public class BellsproutPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Bellsprout";
        public override int ProjType => ModContent.ProjectileType<BellsproutPetProjectileShiny>();
    }
}
