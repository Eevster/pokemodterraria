using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Pokemod.Content.Pets;
using Terraria.Localization;
using Terraria.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
// 0: Hardy
// 1: Lonely
// 2: Adamant
// 3: Naughty
// 4: Brave
// 10: Bold
// 11: Docile
// 12: Impish
// 13: Lax
// 14: Relaxed
// 20: Modest
// 21: Mild
// 22: Bashful
// 23: Rash
// 24: Quiet
// 30: Calm
// 31: Gentle
// 32: Careful
// 33: Quirky
// 34: Sassy
// 40: Timid
// 41: Hasty
// 42: Jolly
// 43: Naive
// 44: Serious
namespace Pokemod.Content.Items.Consumables.Mints
{
    public class Mint : PokemonConsumableItem
    {
        public override void SetDefaults()
        {
            Item.width = 24; // The item texture's width
            Item.height = 24; // The item texture's height

            Item.useTime = 1;
            Item.useAnimation = 1;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item1;

            Item.maxStack = Item.CommonMaxStack; // The item's max stack value
            Item.value = Item.buyPrice(silver: 1); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.

            Item.consumable = true;
        }
    }


    public class HardyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player){
            item.nature = 0;
            
            return false;
        }
    }
    public class LonelyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 1;

            return false;
        }
    }
    public class AdamantMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 2;

            return false;
        }
    }
    public class NaughtyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 3;

            return false;
        }
    }
    public class BraveMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 4;

            return false;
        }
    }
    public class BoldMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 10;

            return false;
        }
    }
    public class DocileMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 11;

            return false;
        }
    }
    public class ImpishMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 12;

            return false;
        }
    }
    public class LaxMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 13;

            return false;
        }
    }
    public class RelaxedMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 14;

            return false;
        }
    }
    public class ModestMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 20;

            return false;
        }
    }
    public class MildMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 21;

            return false;
        }
    }
    public class BashfulMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 22;

            return false;
        }
    }
    public class RashMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 23;

            return false;
        }
    }
    public class QuietMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 24;

            return false;
        }
    }
    public class CalmMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 30;

            return false;
        }
    }
    public class GentleMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 31;

            return false;
        }
    }
    public class CarefulMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 32;

            return false;
        }
    }
    public class QuirkyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 33;

            return false;
        }
    }
    public class SassyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 34;

            return false;
        }
    }
    public class TimidMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 40;

            return false;
        }
    }
    public class HastyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 41;

            return false;
        }
    }
    public class JollyMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 42;

            return false;
        }
    }
    public class NaiveMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 43;

            return false;
        }
    }
    public class SeriousMint : Mint
    {
        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            item.nature = 44;

            return false;
        }
    }
}