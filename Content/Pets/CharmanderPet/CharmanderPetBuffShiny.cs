using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.CharmanderPet
{
	public class CharmanderPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Charmander";
        public override int ProjType => ModContent.ProjectileType<CharmanderPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.ObsidianSkin, 60); // Apply the first buff
            }
        }
	}
}
