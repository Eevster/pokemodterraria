using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MeganiumPet
{
	public class MeganiumPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Meganium";
        public override int ProjType => ModContent.ProjectileType<MeganiumPetProjectile>();
    }

    public class MeganiumPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Meganium";
        public override int ProjType => ModContent.ProjectileType<MeganiumPetProjectileShiny>();
    }
}
