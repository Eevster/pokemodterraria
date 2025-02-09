using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;

namespace Pokemod.Content.Pets.PikachuPet
{
	public class PikachuPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Pikachu";
        public override int ProjType => ModContent.ProjectileType<PikachuPetProjectile>();
	}

    public class PikachuPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Pikachu";
        public override int ProjType => ModContent.ProjectileType<PikachuPetProjectileShiny>();
	}
}
