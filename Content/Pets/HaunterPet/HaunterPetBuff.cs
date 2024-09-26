using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.HaunterPet
{
	public class HaunterPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Haunter";
        public override int ProjType => ModContent.ProjectileType<HaunterPetProjectile>();
    }

    public class HaunterPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Haunter";
        public override int ProjType => ModContent.ProjectileType<HaunterPetProjectileShiny>();
    }
}
