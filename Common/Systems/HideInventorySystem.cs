using System.Collections.Generic;
using Pokemod.Common.Players;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Pokemod.Common.Systems
{
    public class HideInventorySystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
			PokemonPlayer player = Main.player[Main.myPlayer].GetModPlayer<PokemonPlayer>();
			
			if(player.onBattle && player.manualControl){
				int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
				if (inventoryIndex != -1)
				{
					layers[inventoryIndex].Active = false;
				}
			}
        }
    }
}