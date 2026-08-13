using Microsoft.Xna.Framework;
using Pokemod.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace Pokemod.Common.Commands
{
    public class LeaveBattleCommand : ModCommand
    {
        public override string Command => "leavebattle";

        public override CommandType Type => CommandType.Chat;

		public override string Usage => "/leavebattle";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            Player player = caller.Player;

            if (player.GetModPlayer<PokemonPlayer>().onBattle)
            {
                player.GetModPlayer<PokemonPlayer>().SetBattle(false);

			    caller.Reply("You successfully fled the battle", Color.Green);
            }
            else
            {
                caller.Reply("You are not in battle", Color.LightBlue);
            }
        }
    }
}
