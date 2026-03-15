using System;
using System.Collections.Generic;
using System.Linq;
using Pokemod.Content.Items.Accessories;
using Pokemod.Content.Items.Badges;
using Pokemod.Content.Items.Consumables.TMs;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Pokemod.Content.NPCs;
using Pokemod.Content.Items.Accessories.Gems;
using Pokemod.Common.Configs;
using Pokemod.Content.Buffs;

namespace Pokemod.Common.GlobalNPCs
{
	public class SpawnRateNPC : GlobalNPC
	{
        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if(player.HasBuff<PokemonRepel>()) return;

            float multiplier = ModContent.GetInstance<GameplayConfig>().PokemonSpawnMultiplier;
            multiplier = 2f + (multiplier-1f)*0.5f;

            if(multiplier >= 2f){
                spawnRate = (int)(spawnRate / multiplier);
                maxSpawns = (int)(maxSpawns * multiplier);
            }
        }
    }
}