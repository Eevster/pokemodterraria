using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.PidgeotPet
{
	public class PidgeotPetBuff : PokemonPetBuff
	{
        public override string PokeName => "Pidgeot";
        public override int ProjType => ModContent.ProjectileType<PidgeotPetProjectile>();

        public override void UpdateExtraChanges(Player player){

        }
	}

    public class PidgeotPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Pidgeot";
        public override int ProjType => ModContent.ProjectileType<PidgeotPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){

        }
	}
}
