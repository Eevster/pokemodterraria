using Pokemod.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
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
    public abstract class Mint : PokemonConsumableItem
    {
        public virtual int MintNature => 0;
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

        public override bool OnItemUse(Projectile proj)
        {
            bool used = false;
            Player player = Main.LocalPlayer;
            if (player != null)
            {
                CaughtPokemonItem item = null;

                foreach (Item invItem in player.inventory)
                {
                    if (invItem.ModItem is CaughtPokemonItem invPokemon)
                    {
                        if (invPokemon.proj != null)
                        {
                            if (invPokemon.proj == proj)
                            {
                                item = invPokemon;
                                break;
                            }
                        }
                    }
                }
                if (item != null) used = UseMint(item, player);
            }
            Item.consumable = used;
            return used;
        }

        public override bool OnItemInvUse(CaughtPokemonItem item, Player player)
        {
            return UseMint(item, player);
        }

        public bool UseMint(CaughtPokemonItem item, Player player)
        {
            if (item.nature == MintNature)
            {
                string message = Language.GetTextValue("Mods.Pokemod.PokemonInfo.NoEffect");
                CombatText.NewText(player.Hitbox, Color.White, message);
                return false;
            }
            else
            {
                item.nature = MintNature;
                SoundEngine.PlaySound(SoundID.Item2);
                DustBurst(item, player);
                Item.consumable = true;
                ReduceStack(player, Item.type);
                return true;
            }
        }

        public void DustBurst(CaughtPokemonItem item, Player player)
        {
            Vector2 position = player.Center;
            if(item.proj != null)
            {
                position = item.proj.Center;
            }

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(position, 0, 0, DustID.SeaSnail);
            }
        }
    }


    public class HardyMint : Mint
    {
        public override int MintNature => 0;
    }
    public class LonelyMint : Mint
    {
        public override int MintNature => 1;
    }
    public class AdamantMint : Mint
    {
        public override int MintNature => 2;
    }
    public class NaughtyMint : Mint
    {
        public override int MintNature => 3;
    }
    public class BraveMint : Mint
    {
        public override int MintNature => 4;
    }
    public class BoldMint : Mint
    {
        public override int MintNature => 10;
    }
    public class DocileMint : Mint
    {
        public override int MintNature => 11;
    }
    public class ImpishMint : Mint
    {
        public override int MintNature => 12;
    }
    public class LaxMint : Mint
    {
        public override int MintNature => 13;
    }
    public class RelaxedMint : Mint
    {
        public override int MintNature => 14;
    }
    public class ModestMint : Mint
    {
        public override int MintNature => 20;
    }
    public class MildMint : Mint
    {
        public override int MintNature => 21;
    }
    public class BashfulMint : Mint
    {
        public override int MintNature => 22;
    }
    public class RashMint : Mint
    {
        public override int MintNature => 23;
    }
    public class QuietMint : Mint
    {
        public override int MintNature => 24;
    }
    public class CalmMint : Mint
    {
        public override int MintNature => 30;
    }
    public class GentleMint : Mint
    {
        public override int MintNature => 31;
    }
    public class CarefulMint : Mint
    {
        public override int MintNature => 32;
    }
    public class QuirkyMint : Mint
    {
        public override int MintNature => 33;
    }
    public class SassyMint : Mint
    {
        public override int MintNature => 34;
    }
    public class TimidMint : Mint
    {
        public override int MintNature => 40;
    }
    public class HastyMint : Mint
    {
        public override int MintNature => 41;
    }
    public class JollyMint : Mint
    {
        public override int MintNature => 42;
    }
    public class NaiveMint : Mint
    {
        public override int MintNature => 43;
    }
    public class SeriousMint : Mint
    {
        public override int MintNature => 44;
    }
}