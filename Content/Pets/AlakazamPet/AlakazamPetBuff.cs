using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.AlakazamPet
{
	public class AlakazamPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Alakazam";
        public override int ProjType => ModContent.ProjectileType<AlakazamPetProjectile>();
    }

    public class AlakazamPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Alakazam";
        public override int ProjType => ModContent.ProjectileType<AlakazamPetProjectileShiny>();
    }
}
