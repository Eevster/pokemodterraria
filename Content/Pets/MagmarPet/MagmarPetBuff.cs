using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MagmarPet
{
	public class MagmarPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Magmar";
        public override int ProjType => ModContent.ProjectileType<MagmarPetProjectile>();
    }

    public class MagmarPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Magmar";
        public override int ProjType => ModContent.ProjectileType<MagmarPetProjectileShiny>();
    }
}
