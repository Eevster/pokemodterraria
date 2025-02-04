using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MeowthPet
{
	public class MeowthPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Meowth";
        public override int ProjType => ModContent.ProjectileType<MeowthPetProjectile>();
    }

    public class MeowthPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Meowth";
        public override int ProjType => ModContent.ProjectileType<MeowthPetProjectileShiny>();
    }
}
