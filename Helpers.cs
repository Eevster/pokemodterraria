using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace Pokemod
{
    public class ServerText
    {
        public static void SendMessageToPlayer(string text, Player player, Color color = default(Color)){
			if(color == default(Color)) color = Color.White;
            if (Main.netMode == NetmodeID.SinglePlayer){
                Main.NewText(text, color);
            }
            else if(Main.netMode == NetmodeID.MultiplayerClient){
                ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(text), color, player.whoAmI);
            }
        }
    }
}