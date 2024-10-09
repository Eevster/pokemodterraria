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
            if(NPC.downedSlimeKing) MaxWorldLevel = 10;
            if(NPC.downedBoss1) MaxWorldLevel = 15;
			if(NPC.downedBoss2) MaxWorldLevel = 25;
            if(NPC.downedQueenBee) MaxWorldLevel = 30;
			if(NPC.downedBoss3) MaxWorldLevel = 40;
			if(Main.hardMode) MaxWorldLevel = 50;
			if(NPC.downedMechBossAny) MaxWorldLevel = 60;
			if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3) MaxWorldLevel = 70;
			if(NPC.downedPlantBoss || NPC.downedGolemBoss) MaxWorldLevel = 80;
			if(NPC.downedAncientCultist) MaxWorldLevel = 90;
			if(NPC.downedMoonlord) MaxWorldLevel = 100;
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