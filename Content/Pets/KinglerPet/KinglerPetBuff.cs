using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.KinglerPet
{
	public class KinglerPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Kingler";
        public override int ProjType => ModContent.ProjectileType<KinglerPetProjectile>();
    }

    public class KinglerPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Kingler";
        public override int ProjType => ModContent.ProjectileType<KinglerPetProjectileShiny>();
    }
}
