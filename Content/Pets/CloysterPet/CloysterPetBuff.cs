using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CloysterPet
{
	public class CloysterPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Cloyster";
        public override int ProjType => ModContent.ProjectileType<CloysterPetProjectile>();
    }

    public class CloysterPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Cloyster";
        public override int ProjType => ModContent.ProjectileType<CloysterPetProjectileShiny>();
    }
}
