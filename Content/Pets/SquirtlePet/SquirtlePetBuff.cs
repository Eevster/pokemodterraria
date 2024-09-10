using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.SquirtlePet
{
	public class SquirtlePetBuff : PokemonPetBuff
	{
        public override string PokeName => "Squirtle";
        public override int ProjType => ModContent.ProjectileType<SquirtlePetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Flipper, 60); // Apply the first buff
            }
        }
	}
}
