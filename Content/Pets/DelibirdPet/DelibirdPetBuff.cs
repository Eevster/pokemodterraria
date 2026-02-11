using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DelibirdPet
{
	public class DelibirdPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Delibird";
        public override int ProjType => ModContent.ProjectileType<DelibirdPetProjectile>();
    }

    public class DelibirdPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Delibird";
        public override int ProjType => ModContent.ProjectileType<DelibirdPetProjectileShiny>();
    }
}
