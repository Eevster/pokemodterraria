using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MareepPet
{
	public class MareepPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Mareep";
        public override int ProjType => ModContent.ProjectileType<MareepPetProjectile>();
    }

    public class MareepPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Mareep";
        public override int ProjType => ModContent.ProjectileType<MareepPetProjectileShiny>();
    }
}
