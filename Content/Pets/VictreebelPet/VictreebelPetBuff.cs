using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.VictreebelPet
{
	public class VictreebelPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Victreebel";
        public override int ProjType => ModContent.ProjectileType<VictreebelPetProjectile>();
    }

    public class VictreebelPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Victreebel";
        public override int ProjType => ModContent.ProjectileType<VictreebelPetProjectileShiny>();
    }
}
