using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.JynxPet
{
	public class JynxPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Jynx";
        public override int ProjType => ModContent.ProjectileType<JynxPetProjectile>();
    }

    public class JynxPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Jynx";
        public override int ProjType => ModContent.ProjectileType<JynxPetProjectileShiny>();
    }
}
