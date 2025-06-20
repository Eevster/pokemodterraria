﻿using Pokemod.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Pokemod.Content.Items.Consumables
{
    public class MaxRepel : ModItem{
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.consumable= true;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.UseSound = SoundID.Item13;
            Item.maxStack = Item.CommonMaxStack;
            Item.buffType = ModContent.BuffType<PokemonRepel>();
            Item.buffTime = 12*60*60;
            Item.value = Item.buyPrice(silver: 1);
        }
    }
}