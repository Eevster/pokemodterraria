using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MewPet
{
	public class MewPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Mew";
        public override int ProjType => ModContent.ProjectileType<MewPetProjectile>();
    }

    public class MewPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Mew";
        public override int ProjType => ModContent.ProjectileType<MewPetProjectileShiny>();
    }
}
