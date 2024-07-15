using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;

namespace Pokemod.Content.Pets.RaichuPetShiny
{
	public class RaichuPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Raichu";
        public override int ProjType => ModContent.ProjectileType<RaichuPetProjectileShiny>();
        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Shine, 60); // Apply the first buff
                player.AddBuff(BuffID.Swiftness, 60); // Apply the first buff
            }
        }
	}
}
