using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WeedlePet
{
	public class WooperPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Wooper";
        public override int ProjType => ModContent.ProjectileType<WooperPetProjectile>();
    }

    public class WooperPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Wooper";
        public override int ProjType => ModContent.ProjectileType<WooperPetProjectileShiny>();
    }
}
