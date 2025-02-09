using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.IvysaurPet
{
	public class IvysaurPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Ivysaur";
        public override int ProjType => ModContent.ProjectileType<IvysaurPetProjectile>();
	}

    public class IvysaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Ivysaur";
        public override int ProjType => ModContent.ProjectileType<IvysaurPetProjectileShiny>();
	}
}
