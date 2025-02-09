using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.JolteonPet
{
    public class JolteonPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Jolteon";
        public override int ProjType => ModContent.ProjectileType<JolteonPetProjectile>();
    }

    public class JolteonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Jolteon";
        public override int ProjType => ModContent.ProjectileType<JolteonPetProjectileShiny>();
    }
}