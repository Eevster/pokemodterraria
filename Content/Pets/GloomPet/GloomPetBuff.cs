using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GloomPet
{
	public class GloomPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Gloom";
        public override int ProjType => ModContent.ProjectileType<GloomPetProjectile>();
    }

    public class GloomPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Gloom";
        public override int ProjType => ModContent.ProjectileType<GloomPetProjectileShiny>();
    }
}
