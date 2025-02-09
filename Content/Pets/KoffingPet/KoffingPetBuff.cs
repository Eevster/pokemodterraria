using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KoffingPet
{
	public class KoffingPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Koffing";
        public override int ProjType => ModContent.ProjectileType<KoffingPetProjectile>();
    }

    public class KoffingPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Koffing";
        public override int ProjType => ModContent.ProjectileType<KoffingPetProjectileShiny>();
    }
}
