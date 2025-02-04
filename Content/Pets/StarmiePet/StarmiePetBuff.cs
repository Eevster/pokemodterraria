using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.StarmiePet
{
	public class StarmiePetBuff: PokemonPetBuff
	{
        public override string PokeName => "Starmie";
        public override int ProjType => ModContent.ProjectileType<StarmiePetProjectile>();
    }

    public class StarmiePetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Starmie";
        public override int ProjType => ModContent.ProjectileType<StarmiePetProjectileShiny>();
    }
}
