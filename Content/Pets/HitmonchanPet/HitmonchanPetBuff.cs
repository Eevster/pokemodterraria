using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.HitmonchanPet
{
	public class HitmonchanPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Hitmonchan";
        public override int ProjType => ModContent.ProjectileType<HitmonchanPetProjectile>();
    }

    public class HitmonchanPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Hitmonchan";
        public override int ProjType => ModContent.ProjectileType<HitmonchanPetProjectileShiny>();
    }
}
