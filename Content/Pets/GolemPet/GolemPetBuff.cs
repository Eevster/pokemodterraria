using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GolemPet
{
	public class GolemPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Golem";
        public override int ProjType => ModContent.ProjectileType<GolemPetProjectile>();
    }

    public class GolemPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Golem";
        public override int ProjType => ModContent.ProjectileType<GolemPetProjectileShiny>();
    }
}
