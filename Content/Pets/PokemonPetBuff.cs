using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;

namespace Pokemod.Content.Pets
{
	public abstract class PokemonPetBuff : ModBuff
	{
        public virtual string PokeName => "";
        public virtual int ProjType => 0;
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            if(player.ownedProjectileCounts[ProjType] <= 0){
                if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
                    CaughtPokemonItem pokeItem = (CaughtPokemonItem)player.miscEquips[0].ModItem;
                    if(pokeItem.PokemonName == PokeName){
                        int proj = Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center, Vector2.Zero, ProjType, 0, 0f, player.whoAmI);
                        pokeItem.proj = Main.projectile[proj];
                    }
                }
            }
            if(player.ownedProjectileCounts[ProjType] >= 0){
                if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
                    CaughtPokemonItem pokeItem = (CaughtPokemonItem)player.miscEquips[0].ModItem;
                    if(pokeItem.PokemonName == PokeName){
                        pokeItem.SetPetInfo();
                    }
                }
                player.buffTime[buffIndex] = 10;
            }

            UpdateExtraChanges(player);
            
            if(player.ownedProjectileCounts[ProjType] <= 0){
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public virtual void UpdateExtraChanges(Player player){
            
        }
	}
}
