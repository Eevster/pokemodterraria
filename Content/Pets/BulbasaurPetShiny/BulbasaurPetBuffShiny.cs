using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BulbasaurPetShiny
{
	public class BulbasaurPetBuffShiny : PokemonPetBuff
	{
        public override string PokeName => "Bulbasaur";
        public override int ProjType => ModContent.ProjectileType<BulbasaurPetProjectileShiny>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Heartreach, 60); // Apply the first buff
            }
        }
	}
}
