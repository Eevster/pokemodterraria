using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KangaskhanPet
{
	public class KangaskhanPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kangaskhan";
        public override int ProjType => ModContent.ProjectileType<KangaskhanPetProjectile>();
    }

    public class KangaskhanPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kangaskhan";
        public override int ProjType => ModContent.ProjectileType<KangaskhanPetProjectileShiny>();
    }
}
