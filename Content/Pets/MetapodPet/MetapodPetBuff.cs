using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MetapodPet
{
	public class MetapodPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Metapod";
        public override int ProjType => ModContent.ProjectileType<MetapodPetProjectile>();
    }

    public class MetapodPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Metapod";
        public override int ProjType => ModContent.ProjectileType<MetapodPetProjectileShiny>();
    }
}
