using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.AbraPet
{
	public class AbraPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Abra";
        public override int ProjType => ModContent.ProjectileType<AbraPetProjectile>();
    }

    public class AbraPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Abra";
        public override int ProjType => ModContent.ProjectileType<AbraPetProjectileShiny>();
    }
}
