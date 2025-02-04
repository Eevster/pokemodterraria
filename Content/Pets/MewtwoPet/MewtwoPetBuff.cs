using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MewtwoPet
{
	public class MewtwoPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Mewtwo";
        public override int ProjType => ModContent.ProjectileType<MewtwoPetProjectile>();
    }

    public class MewtwoPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Mewtwo";
        public override int ProjType => ModContent.ProjectileType<MewtwoPetProjectileShiny>();
    }
}
