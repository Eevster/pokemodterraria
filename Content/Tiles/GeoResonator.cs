using Microsoft.Build.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemod.Content.Buffs;
using Pokemod.Content.Items;
using Pokemod.Content.Items.Apricorns;
using Pokemod.Content.NPCs;
using Pokemod.Content.Pets;
using Pokemod.Content.TileEntities;
using Pokemod.Content.Tiles.FossilBlocks;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Metadata;
using Terraria.GameContent.ObjectInteractions;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Pokemod.Content.Tiles
{
    public class GeoResonatorTile : ModTile
    {
        public short frameIdleEnd = 3;
        public short frameChargeStart = 104;
        public short frameActivateStart = 9;
        public short frameImpact = 14;
        public short frameLast = 22;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(120, 120, 120), name);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.UsesCustomCanPlace = true;
            /*TileObjectData.newTile.AnchorValidTiles = [
                TileID.Stone,
                TileID.ActiveStoneBlock,
                TileID.InactiveStoneBlock,
                TileID.AmberStoneBlock,
                TileID.Crimstone,
                TileID.Ebonstone,
                TileID.Pearlstone,
                TileID.Sandstone,
                TileID.CorruptSandstone,
                TileID.CrimsonSandstone,
                TileID.HallowSandstone
            ];*/
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.addTile(Type);

            AnimationFrameHeight = 54;

            HitSound = SoundID.MenuTick;
            DustType = -1;
        }

        public override bool CanPlace(int i, int j)
        {
            if (j < Main.worldSurface)
            {
                // If the tile not underground, prevent placement.
                return false;
            }

            return base.CanPlace(i, j);
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<GeoResonator>();
            base.MouseOver(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            SetFrame(i, j, frameChargeStart);
            return true;
        }

        private void SetFrame(int i, int j, int targetFrame)
        {
            var tile = Main.tile[i, j];

            int topX = i - tile.TileFrameX % 54 / 18;
            int topY = j - tile.TileFrameY % 54 / 18;

            for (int x = topX; x < topX + 3; x++)
            {
                for (int y = topY; y < topY + 3; y++)
                {
                    var multiTile = Main.tile[x, y];
                    int frame = multiTile.TileFrameY / AnimationFrameHeight;
                    multiTile.TileFrameY += (short)((targetFrame - frame) * AnimationFrameHeight);
                }
            }
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            SetFrame(i, j, frameChargeStart);
            base.PlaceInWorld(i, j, item);
        }

        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<GeoResonator>());
        }

        public Vector2 DetectFossil(int i, int j)
        {
            int scanRange = 400;
            float distance = scanRange;

            Vector2 scanCenter = new Vector2(i, j);
            Vector2 fossilPosition = scanCenter;

            for (int x = i - scanRange; x < i + scanRange; x++)
            {
                for (int y = j - scanRange; y < j + scanRange; y++)
                {
                    if (y < Main.worldSurface || y > Main.maxTilesY || x < 0 || x > Main.maxTilesX) continue;

                    Vector2 tilePos = new Vector2(x, y);
                    if ((tilePos - scanCenter).Length() < distance)
                    {
                        Tile tile = Main.tile[x, y];
                        ModTile modTile = TileLoader.GetTile(tile.TileType);
                        if (modTile is FossilBlock)
                        {
                            fossilPosition = tilePos;
                            distance = (tilePos - scanCenter).Length();
                        }
                    }
                }
            }
            return fossilPosition;
        }

        private void ImpactEffects(int i, int j)
        {
            Vector2 position = new Vector2(i * 16, j * 16 + 12);

            SoundEngine.PlaySound(SoundID.Item46 with { Pitch = -0.8f , Volume = 0.4f}, position);
            SoundEngine.PlaySound(SoundID.Item37 with { Pitch = -1f, Volume = 0.5f }, position);
            SoundEngine.PlaySound(SoundID.Item89 with { Pitch = -1f }, position);

            for (int k = 0; k < 7; k++)
            {
                int smoke = Dust.NewDust(position, 20, 8, DustID.Smoke, Main.rand.Next(-2, 3), Scale: 1.1f);
                int spark = Dust.NewDust(position, 20, 8, DustID.MagicMirror, Main.rand.Next(-2, 3), Scale: 1f);
                Main.dust[smoke].noGravity = true;
                Main.dust[spark].noGravity = true;
            }

            PunchCameraModifier modifier = new PunchCameraModifier(position, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 3f, 5f, 15);
            Main.instance.CameraModifiers.Add(modifier);

            if (!Main.dedServ) {
                Vector2 fossilLocation = DetectFossil(i, j);
                if ((fossilLocation - new Vector2(i, j)).Length() > 1)
                {
                    int fossilX = (int)((fossilLocation.X - Main.maxTilesX / 2) * 2);
                    int fossilY = (int)((fossilLocation.Y - Main.worldSurface) * 2);
                    string location = Math.Abs(fossilX) + (fossilX > 0 ? (string)Language.GetText("Mods.Pokemod.Items.GeoResonator.East") : (string)Language.GetText("Mods.Pokemod.Items.GeoResonator.West")) + fossilY + (string)Language.GetText("Mods.Pokemod.Items.GeoResonator.Underground");
                    Main.NewText((string)Language.GetText("Mods.Pokemod.Items.GeoResonator.Activation") + location, 255, 206, 163);
                }
                else Main.NewText((string)Language.GetText("Mods.Pokemod.Items.GeoResonator.Failure"), 255, 206, 163);
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            var tile = Main.tile[i, j];
            int frameDiff = 1;
            short frame = (short)(tile.TileFrameY / AnimationFrameHeight);

            if (frame == frameIdleEnd || frame == frameLast)
                frameDiff = 0 - frame;
            if (frame == frameChargeStart)
                frameDiff = frameChargeStart - 100 - frame;

            if (tile.TileFrameX % 54 / 18 == 1 && tile.TileFrameY % 54 / 18 == 2) //Only the bottom center tile should create effects
            {
                if (frame == frameImpact)
                {
                    ImpactEffects(i, j);
                }
                if (frame > frameIdleEnd && frame < frameActivateStart && frame % 1 == 0)
                {
                    SoundEngine.PlaySound(SoundID.Mech with { Pitch = -0.8f, Volume = 0.5f}, new Vector2(i * 16, j * 16));
                }
                if (frame == frameLast) 
                {
                    SoundEngine.PlaySound(SoundID.Item46 with { Pitch = -1f, Volume = 0.2f }, new Vector2(i * 16, j * 16));
                }
            }

            frameYOffset = frameDiff * AnimationFrameHeight;
            tile.TileFrameY += (short)frameYOffset;
        }
    }

    public class GeoResonator : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            // Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.GeoResonatorTile>());

            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 1);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 25)
                .AddRecipeGroup(RecipeGroupID.IronBar, 15)
                .AddIngredient(ItemID.Bone, 5)
                .AddIngredient(ItemID.ManaCrystal, 1)
                .AddIngredient<GeoResonatorBlueprint>(1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }

    public class GeoResonatorBlueprint : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 36;

            Item.material = true;

            Item.ResearchUnlockCount = 1;
            Item.maxStack = Item.CommonMaxStack;

            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 1);
        }
    }
}