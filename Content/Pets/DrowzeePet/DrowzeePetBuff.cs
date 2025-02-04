using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DrowzeePet
{
	public class DrowzeePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Drowzee";
        public override int ProjType => ModContent.ProjectileType<DrowzeePetProjectile>();
    }

    public class DrowzeePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Drowzee";
        public override int ProjType => ModContent.ProjectileType<DrowzeePetProjectileShiny>();
    }
}
