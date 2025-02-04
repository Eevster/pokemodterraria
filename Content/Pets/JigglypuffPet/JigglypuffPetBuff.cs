using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.JigglypuffPet
{
	public class JigglypuffPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Jigglypuff";
        public override int ProjType => ModContent.ProjectileType<JigglypuffPetProjectile>();
    }

    public class JigglypuffPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Jigglypuff";
        public override int ProjType => ModContent.ProjectileType<JigglypuffPetProjectileShiny>();
    }
}
