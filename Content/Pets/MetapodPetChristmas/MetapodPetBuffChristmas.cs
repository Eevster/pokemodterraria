using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.MetapodPetChristmas
{
	public class MetapodPetBuffChristmas: PokemonPetBuff
	{
        public override string PokeName => "Metapod";
        public override int ProjType => ModContent.ProjectileType<MetapodPetProjectileChristmas>();
    }

    
}
