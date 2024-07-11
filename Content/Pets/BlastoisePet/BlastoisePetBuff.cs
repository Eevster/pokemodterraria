using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace Pokemod.Content.Pets.BlastoisePet
{
	public class BlastoisePetBuff :  PokemonPetBuff
	{
        public override string PokeName => "Blastoise";
        public override int ProjType => ModContent.ProjectileType<BlastoisePetProjectile>();

        public override void UpdateExtraChanges(Player player){
            if (player.HasBuff(Type))
            {
                player.AddBuff(BuffID.Gills, 60); // Apply the first buff
                player.AddBuff(BuffID.Flipper, 60); // Apply the first buff
                player.AddBuff(BuffID.Sonar, 60); // Apply the first buff
            }
        }
	}
}
