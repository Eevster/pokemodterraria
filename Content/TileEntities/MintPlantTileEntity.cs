using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Items.Consumables.Mints;
using Pokemod.Content.Tiles;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using static Terraria.ID.ContentSamples.CreativeHelper;

namespace Pokemod.Content.TileEntities
{
    public class MintTileEntity : ModTileEntity
    {
        internal int MintType = -1;
        //internal bool MintChanged;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            
            return tile.HasTile && tile.TileType == ModContent.TileType<MintPlant>();
        }

        public void setMint(int type){
            MintType = type;
            if (Main.netMode == NetmodeID.Server) {
                // The TileEntitySharing message will trigger NetSend, manually syncing the changed data.
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
            }
        }

        public override void NetReceive(BinaryReader reader)
        {
            MintType = reader.ReadInt32();
        }

        public override void NetSend(BinaryWriter writer) {
			writer.Write(MintType);
		}

        public override void SaveData(TagCompound tag)
        {
            tag["MintType"] = MintType;
        }

        public override void LoadData(TagCompound tag)
        {
            MintType = tag.GetInt("MintType");
        }


        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Sync the entire multitile's area.  Modify "width" and "height" to the size of your multitile in tiles
                int width = 1;
                int height = 2;
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