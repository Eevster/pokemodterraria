using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.FlareonPet
{
    public class FlareonPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Flareon";
        public override int ProjType => ModContent.ProjectileType<FlareonPetProjectile>();
    }

    public class FlareonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Flareon";
        public override int ProjType => ModContent.ProjectileType<FlareonPetProjectileShiny>();
    }
}