using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PidgeyPet
{
	public class PidgeyPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Pidgey";
        public override int ProjType => ModContent.ProjectileType<PidgeyPetProjectile>();

        public override void UpdateExtraChanges(Player player){

        }
	}

    public class PidgeyPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Pidgey";
        public override int ProjType => ModContent.ProjectileType<PidgeyPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){

        }
	}
}
