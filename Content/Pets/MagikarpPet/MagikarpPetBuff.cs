using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MagikarpPet
{
	public class MagikarpPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Magikarp";
        public override int ProjType => ModContent.ProjectileType<MagikarpPetProjectile>();
    }

    public class MagikarpPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Magikarp";
        public override int ProjType => ModContent.ProjectileType<MagikarpPetProjectileShiny>();
    }
}
