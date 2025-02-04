using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MrMimePet
{
	public class MrMimePetBuff: PokemonPetBuff
	{
        public override string PokeName => "MrMime";
        public override int ProjType => ModContent.ProjectileType<MrMimePetProjectile>();
    }

    public class MrMimePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "MrMime";
        public override int ProjType => ModContent.ProjectileType<MrMimePetProjectileShiny>();
    }
}
