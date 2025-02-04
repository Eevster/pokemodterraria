using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.ExeggutorPet
{
	public class ExeggutorPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Exeggutor";
        public override int ProjType => ModContent.ProjectileType<ExeggutorPetProjectile>();
    }

    public class ExeggutorPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Exeggutor";
        public override int ProjType => ModContent.ProjectileType<ExeggutorPetProjectileShiny>();
    }
}
