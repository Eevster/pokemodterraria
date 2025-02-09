using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.GrimerPet
{
	public class GrimerPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Grimer";
        public override int ProjType => ModContent.ProjectileType<GrimerPetProjectile>();
    }

    public class GrimerPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Grimer";
        public override int ProjType => ModContent.ProjectileType<GrimerPetProjectileShiny>();
    }
}
