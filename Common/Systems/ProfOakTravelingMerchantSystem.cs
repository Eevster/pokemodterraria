using Microsoft.Xna.Framework;
using Pokemod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.Systems
{
	public class ProfOakTravelingMerchantSystem : ModSystem
	{
		public override void PreUpdateWorld() {
			ProfOakTravelingMerchant.UpdateTravelingMerchant();
		}

		public override void SaveWorldData(TagCompound tag) {
			tag["shopItems"] = ProfOakTravelingMerchant.shopItems;
			if (ProfOakTravelingMerchant.spawnTime != double.MaxValue) {
				tag["spawnTime"] = ProfOakTravelingMerchant.spawnTime;
			}
		}

		public override void LoadWorldData(TagCompound tag) {
			ProfOakTravelingMerchant.shopItems.Clear();
			ProfOakTravelingMerchant.shopItems.AddRange(tag.Get<List<Item>>("shopItems"));
			if (!tag.TryGet("spawnTime", out ProfOakTravelingMerchant.spawnTime)) {
				ProfOakTravelingMerchant.spawnTime = double.MaxValue;
			}
		}

		public override void ClearWorld() {
			ProfOakTravelingMerchant.shopItems.Clear();
			ProfOakTravelingMerchant.spawnTime = double.MaxValue;
		}

        public override void PostWorldGen()
        {
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				var entitySource = new EntitySource_WorldGen("SpawnProfOak");
				Vector2 position = new Vector2(16*Main.spawnTileX, 16*Main.spawnTileY);

				int slot = NPC.NewNPC(entitySource, (int)position.X, (int)position.Y, ModContent.NPCType<ProfOakTravelingMerchant>());
				NPC traveler = Main.npc[slot];
				traveler.homeless = true;
				traveler.direction = Main.spawnTileX >= WorldGen.bestX ? -1 : 1;
				traveler.netUpdate = true;

				if (Main.netMode == NetmodeID.Server && slot < Main.maxNPCs) {
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, slot);
				}
			}
        }
 
		public override void NetSend(BinaryWriter writer) {
			// Note that NetSend is called whenever WorldData packet is sent.
			// We use this so that shop items can easily be synced to joining players
			// We recommend modders avoid sending WorldData too often, or filling it with too much data, lest too much bandwidth be consumed sending redundant data repeatedly
			// Consider sending a custom packet instead of WorldData if you have a significant amount of data to synchronise

			writer.Write(ProfOakTravelingMerchant.shopItems.Count);
			foreach (Item item in ProfOakTravelingMerchant.shopItems) {
				ItemIO.Send(item, writer, writeStack: true);
			}
		}

		public override void NetReceive(BinaryReader reader) {
			ProfOakTravelingMerchant.shopItems.Clear();
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++) {
				ProfOakTravelingMerchant.shopItems.Add(ItemIO.Receive(reader, readStack: true));
			}
		}
	}
}