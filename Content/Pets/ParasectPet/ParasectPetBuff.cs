using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ParasectPet
{
	public class ParasectPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Parasect";
        public override int ProjType => ModContent.ProjectileType<ParasectPetProjectile>();
    }

    public class ParasectPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Parasect";
        public override int ProjType => ModContent.ProjectileType<ParasectPetProjectileShiny>();
    }
}
