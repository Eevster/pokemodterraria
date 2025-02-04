using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VenonatPet
{
	public class VenonatPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Venonat";
        public override int ProjType => ModContent.ProjectileType<VenonatPetProjectile>();
    }

    public class VenonatPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Venonat";
        public override int ProjType => ModContent.ProjectileType<VenonatPetProjectileShiny>();
    }
}
