using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Apricorns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Build.Tasks;
using Terraria.GameContent.Drawing;
using ReLogic.Content;
using Pokemod.Content.Tiles;
using System.IO;
using Terraria.ModLoader.IO;

namespace Pokemod.Content.TileEntities
{
    public class ApricornTileEntity : ModTileEntity
    {
        internal int apricornType = -1;
        //internal bool apricornChanged;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            
            return tile.HasTile && tile.TileType == ModContent.TileType<ApricornPlant>();
        }

        public void setApricorn(int type){
            apricornType = type;
            //apricornChanged = true;
            if (Main.netMode == NetmodeID.Server) {
                // The TileEntitySharing message will trigger NetSend, manually syncing the changed data.
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }

        /*public override void Update() {
			if (apricornChanged) {
				// Sending 86 aka, TileEntitySharing, triggers NetSend. Think of it like manually calling sync.
				NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
				apricornChanged = false;
			}
		}*/

        public override void NetReceive(BinaryReader reader)
        {
            apricornType = reader.ReadInt32();
        }

        public override void NetSend(BinaryWriter writer) {
			writer.Write(apricornType);
		}

        public override void SaveData(TagCompound tag)
        {
            tag["apricornType"] = apricornType;
        }

        public override void LoadData(TagCompound tag)
        {
            apricornType = tag.GetInt("apricornType");
        }


        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 2;
                int height = 3;
                NetMessage.SendTileSquare(Main.myPlayer, i, j, width, height);

                // Sync the placement of the tile entity with other clients
                // The "type" parameter refers to the tile type which placed the tile entity, so "Type" (the type of the tile entity) needs to be used here instead
                NetMessage.SendData(MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
                return -1;
            }

            // ModTileEntity.Place() handles checking if the entity can be placed, then places it for you
            int placedEntity = Place(i, j);
            return placedEntity;
        }

        public override void OnNetPlace()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }
    }
}