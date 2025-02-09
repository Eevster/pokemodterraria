using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VileplumePet
{
	public class VileplumePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Vileplume";
        public override int ProjType => ModContent.ProjectileType<VileplumePetProjectile>();
    }

    public class VileplumePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Vileplume";
        public override int ProjType => ModContent.ProjectileType<VileplumePetProjectileShiny>();
    }
}
