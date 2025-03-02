using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items;
using System;

namespace Pokemod.Content.Pets
{
	public abstract class PokemonPetBuff : ModBuff
	{
        public override string Texture => "Pokemod/Assets/Textures/Pokesprites/Buffs/"+GetType().Name;
        public virtual string PokeName => "";
        public virtual int ProjType => 0;
		public override void SetStaticDefaults() {
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
        {
            bool noProj = false;

            if(player.whoAmI == Main.myPlayer){
                if(player.ownedProjectileCounts[ProjType] <= 0){
                    noProj = true;
                }

                UpdateExtraChanges(player);

                if(noProj){
                    player.DelBuff(buffIndex);
                    buffIndex--;
                }
            }
        }

        public virtual void UpdateExtraChanges(Player player){
            
        }
	}
}
