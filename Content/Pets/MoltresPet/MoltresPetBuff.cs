using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MoltresPet
{
	public class MoltresPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Moltres";
        public override int ProjType => ModContent.ProjectileType<MoltresPetProjectile>();
    }

    public class MoltresPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Moltres";
        public override int ProjType => ModContent.ProjectileType<MoltresPetProjectileShiny>();
    }
}
