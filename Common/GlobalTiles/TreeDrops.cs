using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.Items.Consumables;
using Terraria.Enums;
using Pokemod.Content.NPCs.PokemonNPCs;

namespace Pokemod.Common.GlobalTiles
{
    public class TreeDrops : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type == TileID.Trees && !fail)
            {
                if (Main.rand.NextBool(20)) //1-in-20, or 5% chance
                {
                    Item.NewItem(null, new Vector2(i * 16, j * 16), ModContent.ItemType<ApricornSeed>()); 
                }
                if (Main.rand.NextBool(20)) //1-in-20, or 5% chance
                {
                    Item.NewItem(null, new Vector2(i * 16, j * 16), ModContent.ItemType<OranBerry>()); 
                }
            }
        }

        public override bool ShakeTree(int x, int y, TreeTypes treeType)
        {
            if (treeType == TreeTypes.Forest && WorldGen.genRand.NextBool(20)) {
                int[] treePoke = [
                    ModContent.NPCType<CaterpieCritterNPC>(),
                    ModContent.NPCType<WeedleCritterNPC>(),
                ]; 

				NPC.NewNPC(WorldGen.GetNPCSource_ShakeTree(x, y), x * 16, y * 16, treePoke[WorldGen.genRand.Next(treePoke.Length)]);
				return true;
			}
            return false;
        }
    } 
}