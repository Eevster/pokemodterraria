using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.DragonairPet
{
	public class DragonairPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Dragonair";
        public override int ProjType => ModContent.ProjectileType<DragonairPetProjectile>();
    }

    public class DragonairPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Dragonair";
        public override int ProjType => ModContent.ProjectileType<DragonairPetProjectileShiny>();
    }
}
