using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemod.Content.Items.Apricorns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Build.Tasks;
using Terraria.GameContent.Drawing;
using ReLogic.Content;
using Pokemod.Content.TileEntities;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ObjectInteractions;
using Terraria.GameContent;
using Terraria.Audio;
using Pokemod.Content.Items;
using Pokemod.Content.Pets;
using Pokemod.Content.Buffs;

namespace Pokemod.Content.Tiles
{
    public class PokeHealerTile : ModTile
    {
        private const int FrameWidth = 54; // A constant for readability and to kick out those magic numbers
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(128, 128, 128), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.addTile(Type);

            HitSound = SoundID.MenuTick;
            DustType = -1;
        }

        /*public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) {
			offsetY = 2;
		}*/

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ItemID.Heart;
            base.MouseOver(i, j);
        }

        public override bool RightClick(int i, int j) {
            Player player = Main.LocalPlayer;

            if(player.HasBuff<PokeHealerDebuff>()) return false;

            bool healed = false;

			if (player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance)) { // Avoid being able to trigger it from long range
				player.GamepadEnableGrappleCooldown();
                foreach (Item item in player.inventory)
                {
                    if (item.ModItem is CaughtPokemonItem pokeItem)
                    {
                        HealPokemon(pokeItem, pokeItem.GetPokemonStats()[0], ref healed);
                    }
                }
                if(player.miscEquips[0] != null && !player.miscEquips[0].IsAir){
                    if (player.miscEquips[0].ModItem is CaughtPokemonItem pokeItem)
                    {
                        HealPokemon(pokeItem, pokeItem.GetPokemonStats()[0], ref healed);
                    }
                }
			}

            if(healed){
                SoundEngine.PlaySound(new SoundStyle($"{nameof(Pokemod)}/Assets/Sounds/PKHealer") with {Volume = 0.5f}, player.position);
                player.AddBuff(ModContent.BuffType<PokeHealerDebuff>(), 3*60*60);
            }

			return true;
		}

        public static void HealPokemon(CaughtPokemonItem item, int finalHP, ref bool healed){
            if(item.proj != null){
                if(item.proj.active){
                    if(item.proj.ModProjectile is PokemonPetProjectile proj){
                        if(proj.currentHp < finalHP){
                            proj.currentHp = finalHP;
                            if(item.currentHP < finalHP){
                                item.currentHP = finalHP;
                            }
                            healed = true;
                        }
                        return;
                    }
                }
            }
            if(item.currentHP < finalHP){
                item.currentHP = finalHP;
                healed = true;
            }
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) {
			return true;
		}
    }

    public class PokeHealer : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.PokeHealerTile>());

            Item.width = 40;
            Item.height = 40;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(silver: 50);
        }
        public override void AddRecipes() {
			CreateRecipe()
				.AddIngredient(ItemID.LifeCrystal, 6)
                .AddIngredient(ItemID.ManaCrystal, 5)
                .AddRecipeGroup(RecipeGroupID.IronBar, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
    }
}