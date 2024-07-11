using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharmeleonPetShiny
{
	public class CharmeleonPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charmeleon";
        public override int ProjType => ModContent.ProjectileType<CharmeleonPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
            }
        }
	}
}
