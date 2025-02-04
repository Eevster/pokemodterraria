using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.RapidashPet
{
	public class RapidashPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Rapidash";
        public override int ProjType => ModContent.ProjectileType<RapidashPetProjectile>();
    }

    public class RapidashPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Rapidash";
        public override int ProjType => ModContent.ProjectileType<RapidashPetProjectileShiny>();
    }
}
