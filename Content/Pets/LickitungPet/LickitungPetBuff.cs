using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.LickitungPet
{
	public class LickitungPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Lickitung";
        public override int ProjType => ModContent.ProjectileType<LickitungPetProjectile>();
    }

    public class LickitungPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Lickitung";
        public override int ProjType => ModContent.ProjectileType<LickitungPetProjectileShiny>();
    }
}
