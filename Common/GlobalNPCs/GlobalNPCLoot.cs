using System;
using System.Linq;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.Consumables.TMs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Common.GlobalNPCs
{
    public class GlobalNPCLoot : GlobalNPC
    {
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.WyvernHead)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<AirBalloon>(), 3));
			}

			if (npc.type == NPCID.Mimic)
			{
				int[] TMs = {
					ModContent.ItemType<TMElectric>(), ModContent.ItemType<TMFighting>(), ModContent.ItemType<TMFire>(),
					ModContent.ItemType<TMFlying>(), ModContent.ItemType<TMGhost>(), ModContent.ItemType<TMGrass>(),
					ModContent.ItemType<TMGround>(), ModContent.ItemType<TMIce>(), ModContent.ItemType<TMNormal>(),
					ModContent.ItemType<TMPoison>(), ModContent.ItemType<TMPsychic>(), ModContent.ItemType<TMSteel>(),
					ModContent.ItemType<TMWater>()
				};

				npcLoot.Add(ItemDropRule.OneFromOptions(10, TMs));
			}
		}
    }
}