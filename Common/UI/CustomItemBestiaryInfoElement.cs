using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Newtonsoft.Json.Bson;

namespace Pokemod.Common.UI
{
    public class CustomItemBestiaryInfoElement : IBestiaryInfoElement, IBestiaryPrioritizedElement, ICategorizedBestiaryInfoElement
    {
        public string itemName = "";

        public float OrderPriority => 0.5f;

        public UIBestiaryEntryInfoPage.BestiaryInfoCategory ElementCategory => UIBestiaryEntryInfoPage.BestiaryInfoCategory.Misc;

        public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
        {
            if (!ModContent.TryFind<ModItem>("Pokemod", itemName, out ModItem itemRef) || info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0 || itemName == "")
            {
                return null;
            }
            
            string localizedName = (string)itemRef.DisplayName;
            float panelHeight = 34f;

            UIPanel backPanel = new(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel"), null, customBarSize: 7)
            {
                IgnoresMouseInteraction = true,
                Width = StyleDimension.FromPixelsAndPercent(-14f, 1f),
                Height = StyleDimension.FromPixels(panelHeight),
                BackgroundColor = new Color(43, 56, 101),
                BorderColor = Color.Transparent,
                Left = StyleDimension.FromPixels(-9f),
                HAlign = 1f
            };
            backPanel.SetPadding(1f);

            UIItemIcon icon = new(new Item(itemRef.Type), false)
            {
                HAlign = 0f,
                VAlign = 0.5f,
                Left = StyleDimension.FromPixels(2f),
            };
            backPanel.Append(icon);

            UIText name = new UIText(localizedName)
            {
                Left = StyleDimension.FromPixels(panelHeight + 2f),
                HAlign = 0f,
                VAlign = 0.5f,
                TextColor = Color.White,
                MaxHeight = StyleDimension.FromPixels(5f),
                DynamicallyScaleDownToWidth = true,
            };
            name.SetText(localizedName, 0.8f, false);
            backPanel.Append(name);

            return backPanel;
        }
    }
}
