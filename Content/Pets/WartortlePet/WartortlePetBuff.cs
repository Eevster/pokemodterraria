using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.WartortlePet
{
	public class WartortlePetBuff : PokemonPetBuff
	{
        public override string PokeName => "Wartortle";
        public override int ProjType => ModContent.ProjectileType<WartortlePetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
				player.AddBuff(BuffID.Flipper, 60); // Apply the first buff
				player.AddBuff(BuffID.Gills, 60); // Apply the first buff
            }
        }
	}
}
