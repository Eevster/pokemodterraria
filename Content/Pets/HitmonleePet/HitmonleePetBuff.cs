using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.HitmonleePet
{
	public class HitmonleePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Hitmonlee";
        public override int ProjType => ModContent.ProjectileType<HitmonleePetProjectile>();
    }

    public class HitmonleePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Hitmonlee";
        public override int ProjType => ModContent.ProjectileType<HitmonleePetProjectileShiny>();
    }
}
