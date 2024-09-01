using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Pokemod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Pokemod.Content.Pets.EeveePet;
using Pokemod.Common.Players;

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
					Logger.WarnFormat("GumGumMod: Unknown Message type: {0}", msgType);
					break;
			}
		}
    }
}