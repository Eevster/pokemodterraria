using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Common.Players;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Pokemod.Content;


namespace Pokemod
{
    internal enum PokemodMessageType : byte
	{
		PokemonPlayerSync,
    }
    public class Pokemod : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI) 
		{
			PokemodMessageType msgType = (PokemodMessageType)reader.ReadByte();

			switch (msgType) 
			{
				case PokemodMessageType.PokemonPlayerSync:
					byte playernumber = reader.ReadByte();
					PokemonPlayer pokemonPlayer = Main.player[playernumber].GetModPlayer<PokemonPlayer>();
					pokemonPlayer.ReceivePlayerSync(reader);

					if (Main.netMode == NetmodeID.Server) {
						// Forward the changes to the other clients
						pokemonPlayer.SyncPlayer(-1, whoAmI, false);
					}
					break;
				default:
					Logger.WarnFormat("Pokemod: Unknown Message type: {0}", msgType);
					break;
			}
		}
		
    }
}