using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.TyphlosionPet
{
	public class TyphlosionPetBuff: PokemonPetBuff
	{
        public override string PokeName => "Typhlosion";
        public override int ProjType => ModContent.ProjectileType<TyphlosionPetProjectile>();
    }

    public class TyphlosionPetBuffShiny: PokemonPetBuff
	{
        public override string PokeName => "Typhlosion";
        public override int ProjType => ModContent.ProjectileType<TyphlosionPetProjectileShiny>();
    }
}
