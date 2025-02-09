using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Pokemod.Common.Systems
{
    public class WorldLevel : ModSystem
	{
		public static int MaxWorldLevel = 5;

		public override void ClearWorld() {
			MaxWorldLevel = 5;
		}

        public override void PreUpdateEntities()
        {
            MaxWorldLevel = 5;
            if(NPC.downedSlimeKing) MaxWorldLevel = 7;
            if(NPC.downedBoss1) MaxWorldLevel = 10;
			if(NPC.downedBoss2) MaxWorldLevel = 15;
            if(NPC.downedQueenBee) MaxWorldLevel = 18;
			if(NPC.downedBoss3) MaxWorldLevel = 20;
			if(Main.hardMode) MaxWorldLevel = 25;
			if(NPC.downedMechBossAny) MaxWorldLevel = 30;
			if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) MaxWorldLevel = 35;
			if(NPC.downedPlantBoss || NPC.downedGolemBoss) MaxWorldLevel = 40;
			if(NPC.downedAncientCultist) MaxWorldLevel = 45;
			if(NPC.downedMoonlord) MaxWorldLevel = 50;
        }

        public override void SaveWorldData(TagCompound tag) {
			tag["MaxWorldLevel"] = MaxWorldLevel;
		}

		public override void LoadWorldData(TagCompound tag) {
			MaxWorldLevel = tag.GetInt("MaxWorldLevel");
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write((double)MaxWorldLevel);
		}

		public override void NetReceive(BinaryReader reader) {
			MaxWorldLevel = (int)reader.ReadDouble();
		}
    }
}