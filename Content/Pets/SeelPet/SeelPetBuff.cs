using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SeelPet
{
	public class SeelPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Seel";
        public override int ProjType => ModContent.ProjectileType<SeelPetProjectile>();
    }

    public class SeelPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Seel";
        public override int ProjType => ModContent.ProjectileType<SeelPetProjectileShiny>();
    }
}
