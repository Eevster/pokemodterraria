using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.IvysaurPetShiny
{
	public class IvysaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Ivysaur";
        public override int ProjType => ModContent.ProjectileType<IvysaurPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Heartreach, 60); // Apply the first buff
            }
        }
	}
}
